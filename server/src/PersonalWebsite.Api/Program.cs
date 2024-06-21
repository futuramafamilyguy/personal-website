using Microsoft.AspNetCore.Authentication;
using PersonalWebsite.Api.Authentication;
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

builder.Services.AddMongoClient(builder.Configuration.GetConnectionString("PersonalWebsiteDb"));
builder.Services.AddHostedService<ConfigureMongoDbIndexesService>();
builder.Services.AddCoreServices();
builder.Services.AddInfrastructureServices();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "BasicAuth";
        options.DefaultChallengeScheme = "BasicAuth";
    })
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuth", null);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "AuthenticatedPolicy",
        policy =>
        {
            policy.RequireAuthenticatedUser();
        }
    );
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll");

app.Run();
