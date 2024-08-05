using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PersonalWebsite.Api;
using PersonalWebsite.Api.Authentication;
using PersonalWebsite.Api.Middlewares;
using PersonalWebsite.Api.VisitTracking;
using PersonalWebsite.Core;
using PersonalWebsite.Infrastructure;
using PersonalWebsite.Infrastructure.Data;
using PersonalWebsite.Infrastructure.Images;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbConfiguration>(
    builder.Configuration.GetSection("MongoDbConfiguration")
);
builder.Services.Configure<FileImageStorageConfiguration>(
    builder.Configuration.GetSection("FileImageStorageConfiguration")
);
builder.Services.Configure<BasicAuthConfiguration>(
    builder.Configuration.GetSection("BasicAuthConfiguration")
);
builder.Services.Configure<VisitExclusionConfiguration>(
    builder.Configuration.GetSection("VisitExclusionConfiguration")
);

builder.Services.AddMongoClient(
    builder.Configuration.GetConnectionString("PersonalWebsiteDb"),
    builder.Configuration.GetSection("MongoDbConfiguration").Get<MongoDbConfiguration>()
);
builder.Services.AddHostedService<ConfigureMongoDbIndexesService>();
builder.Services.AddCoreServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddVisitTrackingServices();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        policy =>
            policy
                .WithOrigins(builder.Configuration.GetValue<string>("AllowedOrigin"))
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
    );
});

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("DisableVisitAuth", null)
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("AdminAuth", null)
    .AddCookie(
        CookieAuthenticationDefaults.AuthenticationScheme,
        options =>
        {
            options.Cookie.Name = "admin";
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
            options.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                },
                OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }
            };
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        }
    );

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "DisableVisitPolicy",
        policy =>
        {
            policy.AddAuthenticationSchemes("DisableVisitAuth");
            policy.RequireAuthenticatedUser();
        }
    );

    options.AddPolicy(
        "AdminPolicy",
        policy =>
        {
            policy.RequireClaim("Admin", "true");
        }
    );
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseSession();

app.UseAuthorization();

app.UseMiddleware<IpForwardMiddleware>();

app.UseMiddleware<VisitTrackingMiddleware>();

app.MapControllers();

app.Run();
