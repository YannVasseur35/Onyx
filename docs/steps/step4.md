# Step 4 : Application Service

 Dans les étapes précedentes, nous avons mis en place un endpoint et nous avons créer un projet test sur ce endpoint.

 La suite consiste à mettre en place la couche application de l'architecture en oignon vu à l'étape 1.

 Celle ci va se charger de donner les datas aux couches de présentations, que ce soit notre Api Web, un site Web ou une application mobile/desktop. 

 Nous allons créer un service **WeatherForecastServices** qui implémente l'interface **IWeatherForecastServices**. 

## Implémentation

Revoyons notre controller pourqu'il utilise le service de la couche d'application, qu'on fournir avec de l'injection de dépendance dans le constructeur du controleur.

 ```c#
 public sealed class WeatherForecastsController : ControllerBase
    {
        public readonly IWeatherForecastAppServices _weatherForecastAppServices;

        public WeatherForecastsController(IWeatherForecastAppServices weatherForecastAppServices)
        {
            _weatherForecastAppServices = weatherForecastAppServices;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecastDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_weatherForecastAppServices.GetAllWeatherForecasts());
        }
    }
 ```

 L'interface en question :

 ```c#
    public interface IWeatherForecastAppServices
    {
        Task<IEnumerable<WeatherForecastDto>?> GetAllWeatherForecasts();
    }
 ```

 Et son implémentation : 

 ```c#
    public class WeatherForecastAppServices : IWeatherForecastAppServices
    {
        public async Task<IEnumerable<WeatherForecastDto>?> GetAllWeatherForecasts()
        {
            var mockList = new List<WeatherForecastDto>()
            {
                new WeatherForecastDto()
                {
                     TemperatureC = 19,
                     City = "Paris"
                },
                new WeatherForecastDto()
                {
                     TemperatureC = 24,
                     City = "Rennes"
                }
            };

            return mockList;
        }
    }
```

 En fait, je n'ai fait que déplacer le précédent code du controller vers un service applicatif. Pour le moment les données sont toujours "mockées".

 Si vous executez l'api (ou lancer les tests de l'api) cela va échouer. :heavy_multiplication_x:

```
 System.InvalidOperationException: Unable to resolve service for type 'Onyx.Application.Interfaces.IWeatherForecastAppServices' while attempting to activate 'Onyx.Web.Api.Controllers.WeatherForecastsController'.
```

Traduction : le service qui s'occupe de résoudre les dépendances injectées ne trouve pas d'instance correspondant à IWeatherForecastAppServices, il faut donc lui indiquer quels services sont nécessaires, dans Program.cs de l'Api Web :

```c#
// Add services to the container.
builder.Services.AddSingleton<INotificationsAppServices, NotificationsAppServices>();
builder.Services.AddSingleton<IWeatherForecastAppServices, WeatherForecastAppServices>();
```

INotificationsAppServices va être discuté plus tard. (et le mode Singleton aussi)

## Tests

On va pouvoir ajouter un test dans le projet correspondant Onyx.Application.Tests.

Que test-on ? Le service applicatif est sensé nous fournir des données ou fournir quelconques services. On ne teste pas ces données ci mais le fait qu'on en ait, que l'opération s'est bien déroulée avec toutes les sous-opérations attendues.

```c#
 public class WeatherForecastAppServicesTests
{
    private readonly IWeatherForecastAppServices _weatherForecastAppServices;

    public WeatherForecastAppServicesTests()
    {
        _weatherForecastAppServices = new WeatherForecastAppServices();
    }

    [Fact]
    public async Task GetAllWeatherForecasts_ShouldReturn_NoNull()
    {
        //Arrange

        //Act
        var results = await _weatherForecastAppServices.GetAllWeatherForecasts();

        //Assert
        Assert.NotNull(results);
    }
}
```

Pour le moment c'est trés léger comme test, voir presque inutile. On estime juste qu'il doit y avoir des données en retour (non null).

## Nouveau Service : creation d'un bulletin météo

Envisagons un système de notification qui alerte des gelées pour les maraîcher par exemple. On va tester si la création d'une nouvelle donnée "WeatherForecast" lève une notification quelque part dans le code. On va appeler le service  notificationsAppService:

```c#
public async Task<Operation> CreateWeatherForecasts(WeatherForecastDto weatherForecastDto)
{
    //Todo: creation of weatherForecastDto

    if (weatherForecastDto.TemperatureC <= 0)
    {
        await _notificationsAppService.WeatherAlertAsync("Attention risque de gel", 0, DateTime.UtcNow);
    }

    return new Operation();
}
```

Remarquez que pour le moment on ne s'occupe pas de la création de l'objet. On verra cela plus tard.

Le service renvoit un objet "Operation" qui devra dans la majorité des cas indiquée que l'opération s'est bien déroulée. Dans le cas contraire, on remplira cet objet d'information pour faciliter la maintenance avec le consomateur de ce service. 

Notez qu'on a introduit un nouveau service NotificationsAppServices passé dans le constructeur de WeatherForecastAppServices

```c#
private readonly INotificationsAppServices _notificationsAppService;

public WeatherForecastAppServices(INotificationsAppServices notificationsAppService)
{
    _notificationsAppService = notificationsAppService;
}
```

Ceci apporte un problàme dans nos tests, lors de la création de l'instance dans nos tests. Il nous faut en créer une. 

Or dans le test nous ne voulons pas tester "NotificationsAppServices", nous souhaitons le simuler. Pour cela on peut utiliser un outil comme Moq.

## Moq

Moq est une librairie qui permet de simuler des dépendances externes. Elle doit avoir plein d'autre fonctionnalités mais personnellement je ne l'utilise que pour cela. (je suis pas un expert de cette lib)

https://github.com/devlooped/moq
https://github.com/devlooped/moq/wiki/Quickstart

Par ailleurs, on peut rencontrer tous ces termes Mock, Stub, Dummy, Fake et j'en passe. Personnelement je trouve qu'on rend pas les choses plus simple avec plus de vocabulaire pour dire à peu prêt la même chose. Si vous savez la différence, tant mieux pour vous. Pour moi tout ça veut plus ou moins dire la même chose. J'utiliserai le terme Mock pour la suite. 

Voici notre nouveau constructeur de notre classe de test WeatherForecastAppServicesTests : 

```c#
private readonly IWeatherForecastAppServices _weatherForecastAppServices;
private readonly Mock<INotificationsAppServices> _notificationsAppServices;

public WeatherForecastAppServicesTests()
{
    _notificationsAppServices = new Mock<INotificationsAppServices>();
    _weatherForecastAppServices = new WeatherForecastAppServices(_notificationsAppServices.Object);
}
```

 new Mock\<INotificationsAppServices>(); va créer un service factice sur lequel on va pouvoir intervenir pour lui demander de faire des actions spéciales, afin d'affiner nos tests sur le service que l'on souhaite vraiment tester : WeatherForecastAppServices

 C'est le **Unitaire** de test unitaire. On est censé ne tester qu'une seule chose. 

 Ici on veut tester le fait que si l'on introduit une température de gel (égale ou inférieure à zéro) on doit avoir une notification qui se déclenche. Voici comment faire avec Moq :

```c#
[Fact]
public async Task CreateWeatherForecasts_Should_CallNotificationService_WhenFreezing()
{
    //Arrange
    var mock = new Mock<WeatherForecastDto>();
    mock.Object.TemperatureC = 0;

    //Act
    var operation = await _weatherForecastAppServices.CreateWeatherForecasts(mock.Object);

    //Assert
    Assert.True(operation.IsOperationSuccess);
    _notificationsAppServices.Verify(x => x.WeatherAlertAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
}
```

J'ai utilisé une instance Mocké avec Moq, mais en realité, une instance de l'objet WeatherForecastDto aurait suffit. 
Ce qui serait intéressant, c'est d'avoir un objet dont les propriétés sont remplis aléatoirement. On verra cela plus tard avec AutoFixture https://autofixture.github.io/docs/quick-start/#

Pour information, j'avais préalablement écrit if (weatherForecastDto.TemperatureC < 0) dans le code du service. Ce test m'a permit de me rendre compte que j'avais oublié le 'égale' qui faisait échouer le test. Comme quoi ça sert les tests ! :smile:

