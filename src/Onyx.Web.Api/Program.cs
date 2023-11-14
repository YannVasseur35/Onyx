/**************************************/

var builder = WebApplication.CreateBuilder(args);

//AutoMapper init
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Entity Framework Db Context
builder.Services.AddDbContext<OnyxDbContext>(options =>
    options.UseSqlite(builder.Configuration?.GetConnectionString("DefaultConnection"),
    x => x.MigrationsAssembly("Onyx.Infrastructure"))
);

// Add services to the container.
builder.Services.AddScoped<INotificationsAppServices, NotificationsAppServices>();
builder.Services.AddScoped<IWeatherForecastAppServices, WeatherForecastAppServices>();
builder.Services.AddScoped<IWeatherForecastDataServices, WeatherForecastDataServices>();
builder.Services.AddScoped<IWeatherForecastApiServices, WeatherForecastApiServices>();

//Add http client for OpenWeatherMap external API
builder.Services.AddHttpClient<OpenWeatherMapHttpClient>();

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