using PersonalWebsite.Core;
using PersonalWebsite.Infrastructure;
using PersonalWebsite.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbConfiguration>(builder.Configuration.GetSection("MongoDbConfiguration"));

builder.Services.AddMongoClient(builder.Configuration.GetConnectionString("PersonalWebsiteDb"));
builder.Services.AddHostedService<ConfigureMongoDbIndexesService>();
builder.Services.AddCoreServices();
builder.Services.AddInfrastructureServices();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
