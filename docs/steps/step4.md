# Step 4 : Application Service

 Dans les �tapes pr�cedentes, nous avons mis en place un endpoint et nous avons cr�er un projet test sur ce endpoint.

 La suite consiste � mettre en place la couche application de l'architecture en oignon vu � l'�tape 1.

 Celle ci va se charger de donner les datas aux couches de pr�sentations, que ce soit notre Api Web ou un site Web directement. On pourrait aussi envisager un client WPF ou une application Xamarin qui utilise cette couche. 

 Nous allons cr�er un service **WeatherForecastServices** qui impl�mente l'interface **IWeatherForecastServices**. 

## Impl�mentation

Revoyons notre controller pourqu'il utilise un service de la couche d'application, qu'on fournir avec de l'injection de d�pendance dans le constructeur du controleur.

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

 Et son impl�mentation : 

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

 En fait, je n'ai fait que d�placer le pr�c�dent code du controller vers un service applicatif. Pour le moment les donn�es sont toujours "mock�es".

 Si vous executez l'api (ou lancer les tests de l'api) cela va �chouer. :heavy_multiplication_x:

```
 System.InvalidOperationException: Unable to resolve service for type 'Onyx.Application.Interfaces.IWeatherForecastAppServices' while attempting to activate 'Onyx.Web.Api.Controllers.WeatherForecastsController'.
```

Le service qui s'occupe de r�soudre les d�pendances inject�es ne trouve pas d'instance correspondant � IWeatherForecastAppServices, il faut donc lui indiquer quels services sont n�cessaires, dans Program.cs de l'Api Web :

```c#
// Add services to the container.
builder.Services.AddSingleton<INotificationsAppServices, NotificationsAppServices>();
builder.Services.AddSingleton<IWeatherForecastAppServices, WeatherForecastAppServices>();
```

INotificationsAppServices va �tre discut� plus tard.

## Tests

On va pouvoir ajouter un test dans le projet correspondant Onyx.Application.Tests.

Que test-on ? Le service applicatif est sens� nous fournir des donn�es ou fournir quelconques services. On ne teste pas ces donn�es ci mais le fait qu'on en a, que l'op�ration s'est bien d�roul�e avec toutes les sous-op�rations attendues.

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

Pour le moment c'est tr�s l�ger comme test, voir presque inutile. On estime juste qu'il doit y avoir des donn�es en retour (non null).

## Nouveau Service : creation d'un bulletin m�t�o

Envisagons un syst�me de notification qui alerte des gel�es pour les mara�cher par exemple. On va tester si la cr�ation d'une nouvelle donn�e "WeatherForecast" l�ve une notification quelque part dans le code. On va appeler le service _notificationsAppService:

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

Remarquez que pour le moment on ne s'occupe pas de la cr�ation de l'objet. On verra cela plus tard.

Le service renvoit un objet "Operation" qui devra dans la majorit� des cas indiqu�e que l'op�ration s'est bien d�roul�e. Dans le cas contraire, on remplira cet objet d'information pour faciliter la maintenance avec  consomateur de ce service. 

Notez qu'on a introduit un nouveau service NotificationsAppServices pass� dans le constructeur de WeatherForecastAppServices

```c#
private readonly INotificationsAppServices _notificationsAppService;

public WeatherForecastAppServices(INotificationsAppServices notificationsAppService)
{
    _notificationsAppService = notificationsAppService;
}
```

Ceci apporte un probl�me dans nos tests, lors de la cr�ation de l'instance dans nos tests. Il nous faut en cr�er une. 

Or nous ne voulons pas tester "NotificationsAppServices", nous souhaitons le simuler. Pour cela on peut utiliser un outil comme Moq.

## Moq

https://github.com/devlooped/moq
https://github.com/devlooped/moq/wiki/Quickstart

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

 new Mock\<INotificationsAppServices>(); va cr�er un service factice sur lequel on va pouvoir intervenir pour lui demander de faire des actions sp�ciales, afin d'affiner nos tests sur le service que l'on souhaite vraiment tester : WeatherForecastAppServices

 C'est le **Unitaire** de test unitaire. On ne test **QUE** une seule chose.

 Ici on veut tester le fait que si l'on introduit une temp�rature de gel (�gale ou inf�rieure � z�ro) on doit avoir une notification qui se d�clenche. Voici comment faire avec Moq :

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

Pour information, j'avais pr�alablement �crit if (weatherForecastDto.TemperatureC < 0) dans le code du service. Ce test m'a permit de me rendre compte que j'avais oubli� le '�gale' qui faisait �chouer le test. Comme quoi �a sert. :smile:

