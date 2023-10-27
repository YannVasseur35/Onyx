# Step 9 : Finalisation de la chaine

On vient de mettre en place notre ORM, il nous manque pas grand chose pour terminer notre chaine d'information de la base de donnée vers l'API

Remplaçons les données en dur dans le code par celle en base de données, dans le DataService. On lui fournit un context de données (EF) qui ira chercher tous les WeatherForecasts. 

Notez qu'on a un mapping entre notre Entity EF et notre objet Core.

```C#
private readonly OnyxDbContext _context;
private readonly IMapper _mapper;

public WeatherForecastDataServices(OnyxDbContext context, IMapper mapper)
{
    _context = context;
    _mapper = mapper;
}

public async Task<IEnumerable<WeatherForecast>?> GetAllAsync()
{
    return _context.WeatherForecasts.Select(x => _mapper.Map<WeatherForecast>(x));
}
```

De la meme manière que le mapping dans Onyx.Application vu précédement, on va rajouter un mapping entre un objet Core (Domain) et un objet Entity (DAO)

AutoMapper va detecter ce nouveau profil et le gérer.

## Erreur

Si on compile ça va. Mais à l'execution je vais avoir cette erreur :

```
System.AggregateException : 'Some services are not able to be constructed (Error while validating the service descriptor 'ServiceType: Onyx.Application.Interfaces.IWeatherForecastAppServices Lifetime: Singleton ImplementationType: Onyx.Application.Services.WeatherForecastAppServices': Cannot consume scoped service 'Onyx.Infrastructure.Datas.OnyxDbContext' from singleton 
```

Rappel, on a pour le moment déclarer notre injection de dépendance en Singleton dans le Program.cs

```C#
builder.Services.AddSingleton<INotificationsAppServices, NotificationsAppServices>();
builder.Services.AddSingleton<IWeatherForecastAppServices, WeatherForecastAppServices>();
builder.Services.AddSingleton<IWeatherForecastDataServices, WeatherForecastDataServices>();
```

L'erreur nous informe que OnyxDbContext doit être dans un service de type Scoped.

Il y a trois manière de gérer les instances en injection de dépendance (en utilisant la lib de base Microsoft.Extensions.DependencyInjection) :

- Singleton : instance disponible pendant toute l'execution du program. Utile pour des tâches en background par exemple. 
- Transient : une instance est toujours créée et aussitôt détruite après utilisation
- Scoped : Entre Singleton et Transicient. On définie un scope qui aura son instance et ne vivra que dans ce scope. 

https://learn.microsoft.com/fr-fr/dotnet/core/extensions/dependency-injection-usage

```C#
builder.Services.AddScoped<INotificationsAppServices, NotificationsAppServices>();
builder.Services.AddScoped<IWeatherForecastAppServices, WeatherForecastAppServices>();
builder.Services.AddScoped<IWeatherForecastDataServices, WeatherForecastDataServices>();
```
Pour faire simple, tout est Scoped, sauf si c'est nécessaire d'avoir un Singleton (global à l'app) ou à l'inverse un Transient (nouvelle instance à coup sûr). 

Maintenant l'application tourne !

## Tests d'infrastructure

Mais on a tout pété nos tests... 

On a modifié le constructeur de WeatherForecastDataServices, du coup notre test est cassé. 

Dans ce test Onyx.Infrastructure.Tests on a besoin d'une base de données et d'un context EF. Pas bien pratique me direz vous pour des tests auto. 

Il existe un package Microsoft.EntityFrameworkCore.InMemory qui permet de mettre une base de donnée en mémoire. Ceci va s'avérer très pratique, car à l'avenir, ces tests devront tourner via des pipeplines sur Azure Devops. Autant ne pas s'embêter à monter un environnement complet de test avec base de données si c'est possible.

On va donc créer notre propre context de données en mémoire. 

On a besoin aussi d'un mapper qu'on va initialiser dans le constructeur.

```C#
public class WeatherForecastDataServicesTests
{
    private readonly IMapper _mapper;

    private DbContextOptions<OnyxDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<OnyxDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    public WeatherForecastDataServicesTests()
    {
        var config = new MapperConfiguration(c => {               
            c.AddProfile<Mappings.MappingProfiles>();
        });

        _mapper = config.CreateMapper();             
    }

    [Fact]
    public async Task GetAllWeatherForecasts_ShouldReturn_Data()
    {
        //Arrange
        using (var context = new OnyxDbContext(CreateNewContextOptions()))
        {
            context.WeatherForecasts.Add(new WeatherForecastEntity { City = "Rennes", TemperatureC = 24 });
            context.SaveChanges();

            var weatherForecastDataServices = new WeatherForecastDataServices(context, _mapper);
            var results = await weatherForecastDataServices.GetAllAsync();

            Assert.NotNull(results);
            Assert.True(results.Count() > 0);
            Assert.True(results.FirstOrDefault()?.City == "Rennes");
            Assert.True(results.FirstOrDefault()?.TemperatureC == 24);
        }
    }
}
```
## Tests de la web API

Le test de l'API foire. Maintenant qu'on a implémenter toute la chaine, et que le test lance le Program.cs en mémoire qui lui a besoin d'une base de données, ca plante

```
Microsoft.Data.Sqlite.SqliteException (0x80004005): SQLite Error 14: 'unable to open database file'.
```

On va donc configurer un nouveau Context via une IClassFixture. On commence par enlever le contexte présent (celui avec un fichier) et on rajoute le notre en mémoire.

```C#
public class AppTestFixture : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // remove the existing context configuration
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<OnyxDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<OnyxDbContext>(options =>
                options.UseInMemoryDatabase("TestDB"));
        });
    }
}
```



