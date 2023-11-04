# Step 6 : Mapping

Dans l'étape précédente, nous avons fait une extension de mapping d'objet entre notre model domain et notre objet Dto :

```c#
 public static class Mapping
 {
     public static WeatherForecastDto ToWeatherForecastDto(this WeatherForecast model)
     {
         return new WeatherForecastDto
         {
             Date = model.Date,
             TemperatureC = model.TemperatureC,
             City = model.City,
             //(...)
         };
     }

     public static WeatherForecast ToWeatherForecast(this WeatherForecastDto dto)
     {
         return new WeatherForecast
         {
             Date = dto.Date,
             TemperatureC = dto.TemperatureC,
             City = dto.City,
             //(...)
         };
     }
 }
```

C'est pas une mauvaise façon de faire, mais cette méthode a un problème majeur. Que se passe t-il si on rajoute une propriété à l'un des deux objets ?

Il faudra mapper manuellement celle ci dans ce cette classe. Suffit d'y penser. Mais vous allez oubliez, c'est probable. 
Un autre dev ne saura pas forcément que c'est la. Pire, tout ça peut partir en prod avant de vous appercevoir qu'une propriété est vide ou null. Alors vous le verrez à un moment ou à un autre, mais vous aller chercher. Et puis c'est pas marrant à faire. 

Pourquoi pas l'automatiser ?

## AutoMapper

Fini l'extension, j'ai rajouté un attribut [Obsolete] dans cette classe pour le moment afin de l'avoir dans cette branche git, mais à l'avenir elle va dégager.

On va utiliser Automapper, qui comme son nom l'indique permet de mapper deux objets.  
AutoMapper est open source : https://docs.automapper.org/en/latest/Getting-started.html, On ajoute ce package Nuget dans le projet Onyx.Application

J'utilise aussi AutoMapper.Extensions.Microsoft.DependencyInjection qui me permet de référencer les profiles d'automapper automatiquement en une ligne dans le Program.cs
 
```c#
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
```

Ensuite il faut injecter dans notre classe WeatherForecastAppServices une instance de l'interface d'automapper IMapper.

```c#
private readonly INotificationsAppServices _notificationsService;
private readonly IWeatherForecastDataServices _weatherForecastDataServices;
private readonly IMapper _mapper;

public WeatherForecastAppServices(
    INotificationsAppServices notificationsService,
    IWeatherForecastDataServices weatherForecastDataServices,
    IMapper mapper)
{
    _notificationsService = notificationsService;
    _weatherForecastDataServices = weatherForecastDataServices;
    _mapper = mapper;
}
 
public async Task<IEnumerable<WeatherForecastDto>?> GetAllWeatherForecasts()
{
    var weatherForecastList = await _weatherForecastDataServices.GetAllAsync();

    return weatherForecastList.Select(x => _mapper.Map<WeatherForecastDto>(x));
}
```

Notez le changement : _mapper.Map<WeatherForecastDto>(x)
On demande au mapper de nous donner un objet dto depuis un objet model domain. C'est l'équivalent de "ToWeatherForecastDto" de tout à l'heure.

Automapper utilise des "Profiles" de convertion qu'il va falloir définir dans une classe, comme celle ci 

```c#
 public class MappingProfiles : Profile
 {
     public MappingProfiles()
     {
         CreateMap<WeatherForecast, WeatherForecastDto>();

         CreateMap<WeatherForecastDto, WeatherForecast>();
     }
 }
```

Ici on dit simplement qu'on référence un profil de conversion de la classe WeatherForecast (source) en WeatherForecastDto (destination). Et inversemement pour la ligne suivante.

En gros, c'est la règle la plus simple, automapper va identifier via de la réfléxion chaque propriété de la source pour voir si dans la destination on a pas la même chose et affecter la valeur. 

Ainsi, si on ajoute une même propriété dans chacune des deux classes, celle ci sera "recopiée" automatiquement. PLus besoin de mapper à la main !

Bien entendu on aura des profiles plus élaboré. On verra comment ignorer certaine propriété ou mapper des champs "à plat" vers des sous objets. AutoMapper permet de faire pas mal de chose. On mettra aussi quelques tests.


## Encore un test qui pète

Ca va devenir une habitude, le constructeur de WeatherForecastAppServicesTests ayant encore changé, les tests qui l'utilise vont planter aussi. Cependant les tests eux même demeurent inchangés. 

Le test GetAllWeatherForecasts_ShouldReturn_NoNull est vraiment trop basique. Il n'a pas besoin de mapper les objets car il n'y en a pas dans le service WeatherForecastDataServices mocké. (rien dans Arrange)

On va donc créer ce test et fournir des données de test en simulant le service de données pour qu'il nous renvoit exactement ce dont on a besoin pour notre test :

```c#
[Fact]
public async Task GetAllWeatherForecasts_ShouldReturn_2Items()
{
    //Arrange
    IEnumerable<WeatherForecast>? mocks = new List<WeatherForecast> {
        new WeatherForecast() { City = "Paris", TemperatureC = 22},
        new WeatherForecast() { City = "Rennes", TemperatureC = 16},
    };

    _weatherForecastDataServices.Setup(x => x.GetAllAsync()).ReturnsAsync(mocks);

    //Act
    var results = await _weatherForecastAppServices.GetAllWeatherForecasts();

    //Assert
    Assert.NotNull(results);
    Assert.Equal<int>(mocks.Count(), results.Count());
}
```

On "arrange" les données pour que l'on ait deux valeurs fournies par notre dataService. On test que l'on ait bien 2 valerus avec notre service applicatif. 

Problème, on ne teste pas le fait que toutes les données soient vraiment mappée. 
On va le faire dans une autre classe de test "MappingTests"
 
## Test du mapping pure

```c#
public class MappingTests
{
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _config;

    public MappingTests()
    {
        _config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfiles>();
        });

        _mapper = _config.CreateMapper();
    }

    [Fact]
    public void AutoMapper_Configuration_IsValid()
    {
        //arrange

        //act

        //assert
        _config.AssertConfigurationIsValid();
    }

    [Fact]
    public void AutoMapper_Map_WeatherForecast_To_WeatherForecastDto()
    {
        //arrange
        var model = new WeatherForecast()
        {
            City = "Rennes",
            TemperatureC = 1,
            Summary = "fait pas chaud",
        };

        //act
        var dto = _mapper.Map<WeatherForecastDto>(model);

        //assert
        Assert.NotNull(dto);
        Assert.Equal(model.City, dto.City);
        Assert.Equal(model.TemperatureC, dto.TemperatureC);
        Assert.Equal(model.Summary, dto.Summary);
    }

    [Fact]
    public void AutoMapper_Map_WeatherForecastDto_To_WeatherForecast()
    {
        //arrange
        var dtoMock = new WeatherForecastDto()
        {
            City = "Rennes",
            TemperatureC = 1,
            Summary = "fait pas chaud",
        };

        //act
        var model = _mapper.Map<WeatherForecast>(dtoMock);

        //assert
        Assert.NotNull(model);
        Assert.Equal(dtoMock.City, model.City);
        Assert.Equal(dtoMock.TemperatureC, model.TemperatureC);
        Assert.Equal(dtoMock.Summary, model.Summary);
    }
}
```

J'ai écris 3 tests, le premier utilise AssertConfigurationIsValid d'une configuration (d'un profil de mapping) et va detecter des problèmes d'associations entre propriétés qui n'ont pas le même nom. Voir la doc https://docs.automapper.org/en/stable/Configuration.html

Les deux autres sont très simples, on met en entrée un objet qu'on mappe et ensuite on vérifie qu'en sortie on a les mêmes propriétés précédement initialisées.

Alors en effet, on ne teste ici pas toutes les propriétés, par féniantise. Cependant c'est long et on retombe dans le même problème que tout à l'heure; qu'est ce qu'on fait si on rajoute une propriété ? on modifie le test ? Pourquoi pas, tant que l'on ne touche ni à Arrange, ni à Act, seulement à Assert. Sinon on écrit un nouveau test.

On part du principe que si quelques propriétés sont bien mappé, le reste devrait être ok. AutoMapper a prévu le coup.
On a un autre test AssertConfigurationIsValid qui valide l'ensemble des mappings et nous avertira des éventuels problèmes. 

En vrai tester le fait que l'on recopie bien les propriétés est une bonne chose. Mais ce qui est plus important, c'est de tester ce qu'on ne veut pas mapper ! Ou tester des mappages spéciaux. On va voir cela :

## Mappings spéciaux

Je vais modifier mon objet WeatherForecast pour l'exemple. Créons une classe Coords qui contient la longitude et la latitude. Ensuite on modifie WeatherForecast et on lui rajoute une propriété Coords (et on vire latitude et longitude) 

On ajoute de suite deux tests (pour le deux sens de mapping)

```C#
[Fact]
public void AutoMapper_Map_Coords_WeatherForecast_To_WeatherForecastDto()
{
    //arrange
    var model = new WeatherForecast()
    {
        Coords = new()
        {
            Latitude = 44,
            Longitude = 55
        }
    };

    //act
    var dto = _mapper.Map<WeatherForecastDto>(model);

    //assert
    Assert.NotNull(dto);
    Assert.Equal(model.Coords.Latitude, dto.Latitude);
    Assert.Equal(model.Coords.Longitude, dto.Longitude);
}

[Fact]
public void AutoMapper_Map_Coords_WeatherForecastDto_To_WeatherForecast()
{
    //arrange
    var dtoMock = new WeatherForecastDto()
    {
        Latitude = 12,
        Longitude = 13
    };

    //act
    var model = _mapper.Map<WeatherForecast>(dtoMock);

    //assert
    Assert.NotNull(model);
    Assert.Equal(dtoMock.Latitude, model.Coords.Latitude);
    Assert.Equal(dtoMock.Longitude, model.Coords.Longitude);
}
```

A ce stade, si on lance un test, ces deux derniers sont en échecs. (et c'est normal)
On a aussi un autre test en échec, le premier AutoMapper_Configuration_IsValid. En fait si on doit en garder qu'un ça serait celui la. 

Le piège avec AutoMapper, c'est que tout compile et que ça s'executera. Sauf que vous n'aurez plus de valeurs pour les coordonnées géographique car AutoMapper ne sait pas faire le lien. 

## Règle de profile AutoMapper  

Voici comment corriger cela.Dans le mapping Model => Dto, on lui dit que pour le membre (ForMember) Latitude, tu me mappes ça sur Coordinates.Latitude. Idem pour Longitude.

```c#
public MappingProfiles()
{
    CreateMap<WeatherForecast, WeatherForecastDto>()
        .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Coordinates.Latitude))
        .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Coordinates.Longitude));

    CreateMap<WeatherForecastDto, WeatherForecast>()
        .ForPath(dest => dest.Coordinates.Latitude, opt => opt.MapFrom(src => src.Latitude))
        .ForPath(dest => dest.Coordinates.Longitude, opt => opt.MapFrom(src => src.Longitude));
}
```

L'inverse de fonctionnera pas avec ForMember, il faudra utiliser ForPath à la place de ForMember.

Relancer les tests, normalement tout passe !

# Conclusion

Le mapping automatique c'est bien quand on sait ce qu'on fait et que ça reste "pratique" et "simple".

Sur certaine classe, ça peut devenir vite le bordel, dans ce cas, faites vos propres mappings manuels et tester les !

Pour ma part, je trouve AutoMapper vraiment pratique, surtout dans des projets style MVP (Most Valuable Product: projet alpha pour test rapide sur marché) ou l'on souhaite aller vite, très vite. 

Alternative Mapster https://github.com/MapsterMapper/Mapster