# Api Open Weather Map

On va pas se le cacher, notre exemple d'Api basé sur l'exemple d'un projet par default .net proposé par Visual Studio, n'est pas nouveau et à vrai dire, cela existe déjà sur le net. 

Dans cette étape in va proposer un service dans notre API qui renvoit un bulletin météo d'une ville. 

On pourrait le faire avec l'existant, c'est à dire peupler notre base de données avec un ensemble de données ville/température et retourner ces infos via un controleur à nous. 

L'idée ici c'est d'implémenter un nouveau service tiers, qui nous fournit ces données en live et les retourner via notre controler. On va faire une sorte de proxy entre le client et l'API tierce.

Pour cela, on va utiliser Open Weather Map qui propose de services de données météorologiques.


## OpenWeatherMap

Open Weather Map https://openweathermap.org/  que je vais appeler OWM pour la suite est une Api Web consultable gratuitement (jusqu'à un certain point : https://openweathermap.org/price). La version Free est largement suffisante pour notre exemple. 

Pour l'utiliser il faut créer un compte pour obtenir une clé API. Pour info celle ci mettra quelques heures à être opérationelle. 

Nous allons :
- créer un nouveau endpoint sur notre controler Web API à nous
- créer une nouvelle methode applicative pour répondre à ce besoin
- créer un nouveau service d'infrastructure pour aller chercher les données sur OWM

J'ai créé un objet OpenWeatherMapResponseDto (dans le Core) qui est mappée directement sur le données OWM. C'est fait à l'arrache, ya pas toutes les propriétés. Pour le moment cet objet n'est pas mappé sur notre objet à nous WeatherForecastDto. On fournira donc directement les données d'OWM via notre API.
Pour faire propre, on aurait pu mapper les infos OpenWeatherMapResponseDto <==> WeatherForecastDto mais c'est pas l'objectif ici. 

## Le controler 

```c#
        [HttpGet]
        [Route("/city/{city}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OpenWeatherMapResponseDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByCyAsync(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest();
            }
 
            OpenWeatherMapResponseDto? res = await _weatherForecastAppServices.GetWeatherForecast(city);

            return Ok(res);
        }
```

Rien d'extraordinaire ici, on utilise le service applicatif existant _weatherForecastAppServices qui a une nouvelle signature GetWeatherForecast(city).

## App Service

```c#
 public async Task<OpenWeatherMapResponseDto>? GetWeatherForecast(string city)
 {
     OpenWeatherMapResponseDto openWeatherMapResponseDto = await _weatherForecastApiServices.GetForecastAsync(city);

     return openWeatherMapResponseDto;
 }
```

On a implémenté un nouveau service de IWeatherForecastApiServices qui a une méthode GetForecastAsync(city). Ce service se situe dans le projet Infrastructure. 

## Infrastructure

```c#
    public class WeatherForecastApiServices : IWeatherForecastApiServices
    {
        private readonly OpenWeatherMapHttpClient _httpClient;
        private readonly string _apiKey = "";

        public WeatherForecastApiServices(OpenWeatherMapHttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenWeatherMapApiKey"] ?? "";
        }

        public async Task<OpenWeatherMapResponseDto?> GetForecastAsync(string city)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                 throw new ArgumentNullException(paramName: "_apiKey", message: "OpenWeatherMapApiKey must not be null");
            }

            string request = $"{_httpClient.GetBaseUrl()}/?appid={_apiKey}&q={city}";

            HttpResponseMessage httpResponse = await _httpClient.HttpClient.GetAsync(request);

            string response = await httpResponse.Content.ReadAsStringAsync();

            var res = JsonSerializer.Deserialize<OpenWeatherMapResponseDto?>(response);

            return res;
        }
    }
```

Ce code permet via une requete http (OpenWeatherMapHttpClient) d'aller chercher les infos sur l'API OWS.

Notez que pour cela on a besoin d'une clé 'ApiKey'. Elle se situe dans le fichier appsetting.json. Cette façon de faire, c'est à dire mettre une clé API dans un fichiers de settings pose problème, car elle va se retrouver visible de tous dans le repo Git. Sur un repo privé, c'est moins génant. Sur un repo public ça peut devenir problématique. Nous verrons comment gérer cela dans la prochaine étape. 


## Tests

On rajoute une classe de test : 

```c#
public class WeatherForecastApiServicesTests
{
    private readonly IWeatherForecastApiServices _weatherForecastApiServices;

    public WeatherForecastApiServicesTests()
    {
        var builder = new ConfigurationBuilder();

        var appSettings = @"{
            ""OpenWeatherMapApiBaseUrl"" : ""https://api.openweathermap.org/data/2.5/weather"",
            ""OpenWeatherMapApiKey"" : ""2ab3de3a263b8b265877be0c4acf25b3""
        }";
        builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));

        var configuration = builder.Build();

        HttpClient _httpClient = new HttpClient();
        var openWeatherMapHttpClient = new OpenWeatherMapHttpClient(_httpClient, configuration);

        _weatherForecastApiServices = new WeatherForecastApiServices(openWeatherMapHttpClient, configuration);
    }

    [Fact]
    public async Task GetMonitorsAsync_ShouldReturn_NotNull()
    {
        //Arrange

        //Act
        var response = await _weatherForecastApiServices.GetForecastAsync("Rennes");

        //Assert
        Assert.NotNull(response);
        Assert.True(response.current?.temp > -50);
    }
}
```

Notez que dans notre test, on devrait fournir un fichier appSetting.json. Ici on ajoute ces settings directement en mémoire sans fichier. On utilise un package pour AddJsonStream : Microsoft.Extensions.Configuration.Json;

Encore une fois, c'est pas cool d'avoir la clé API en clair dans ce test. On est pas DevSecOps !!!

# Conclusion

On a rajouté plusieurs couches de services afin d'aller chercher des données sur une API tierce et les fournir via notre APi Web. Le service applicatif utilise le nouveau service d'infrastructure WeatherForecastApiServices. Si un jour on souhaite récupérer des données via une autre Api Web, on aura que cette partie infrastructure à modifier. 

L'API tierce OWM utilise une clé API que l'on stocke dans un fichier appsettings.json. Cette pratique n'est pas secure. Ajouter sur le repo git des clés, des passwords ou données sensible est à risque. Et meme si vous les effacez à l'avenir dans un nouveau commit, git garde tout et on pourra les retrouver (ya des techniques mais c'est relou)

On verra dans la prochaine étape comment rendre cela plus 'DevSecOps'. 