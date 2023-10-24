using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Onyx.Core.Interfaces;
using Onyx.Infrastructure.Datas;
using Onyx.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

//AutoMapper init
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Entity Framework Db Context
builder.Services.AddDbContext<OnyxDbContext>();

// Add services to the container.
builder.Services.AddSingleton<INotificationsAppServices, NotificationsAppServices>();
builder.Services.AddSingleton<IWeatherForecastAppServices, WeatherForecastAppServices>();
builder.Services.AddSingleton<IWeatherForecastDataServices, WeatherForecastDataServices>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/**************************************/

//DB Inits
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    OnyxDbInitializer.CreateDbIfNotExists(scope, logger);
}

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

// Make the implicit Program class public so test projects can access it
public partial class Program
{ }