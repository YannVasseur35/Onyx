# Step 2 : Premier endpoint Api Rest

Un endpoint est une "terminaison", ou une url d'une api qui permet de gérer un resource. 

Dans notre exemple on va gérer des bulletins météos (WeatherForecast) via notre api. 

Par exemple lorsque l'on tapera l'url https://mondomain.com/api/weatherforecasts on devrait recevoir un ensemble de bulletins meteo sous forme de json.

Pour commencer on va créer un objet DTO pour 'Data Transfert Object'. Comme son nom l'indique, il nous sert d'objet de transport de donnée, en général entre une API web et un site web. 

Pourquoi créer un objet spécial ? 
La principale raison est d'envoyer que le strict nécessaire. Vous n'avez pas besoin d'envoyer des données inutiles ou compromettantes (id, password, clé...). Il faut créer un objet sur mesure qui répond au besoin, point. 

Dans un projet monolith ou legacy, on avait tendance à utiliser un objet du domain (métier) voir même un objet d'accès aux données (data object) qui lui expose d'autres données comme des cléfs étrangères, des dates et que sais-je encore. 

Il faudra à un moment faire correspondre les données entre ces objets (data - domain - dto) et pour cela on va les "mapper", soit à la main (non recommandé) soit via une lib externe comme AutoMapper. Ce sujet sera aborder plus tard.

Considérez un objet dto comme un object publique à destination d'un tiers indépendants, qui fera ce qu'il veut de cette donnée.

Un bulletin météo c'est 
- des informations techniques (température, hydrométrie, pression atmo, etc) On va prendre que la température. 
- attribué à un lieu et à un moment (un datetime) 

```c#
namespace Onyx.Application.Dtos
{
    public class WeatherForecastDto
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public string? Summary { get; set; }
        public bool Current { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
```

On verra plus tard que cet objet peut être optimisé car les données du lieu son immuable. On pourrait utiliser un id qui référence une donnée d'une liste de lieu qui peut être en cache ou locale coté client, ce qui améliorait les performances sur de gros volume. C'est pas le sujet pour le moment.

Voici le code de notre controler, très basique. Notez qu'on mock (simule) deux dtos, température de Rennes et Paris, sans les autres informations. 

```c#
namespace Onyx.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class WeatherForecastsController : ControllerBase
    {
        public WeatherForecastsController()
        {
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecastDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<WeatherForecastDto> mockList = new List<WeatherForecastDto>()
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

            return Ok(mockList);
        }
    }
}
```

Lancer votre projet et rendez vous sur ce endpoint : /api/WeatherForecasts, voici ce que vous devez obtenir.

```json
[
  {
    "date": "0001-01-01T00:00:00",
    "temperatureC": 19,
    "temperatureF": 0,
    "summary": null,
    "current": false,
    "country": null,
    "city": "Paris",
    "latitude": 0,
    "longitude": 0
  },
  {
    "date": "0001-01-01T00:00:00",
    "temperatureC": 24,
    "temperatureF": 0,
    "summary": null,
    "current": false,
    "country": null,
    "city": "Rennes",
    "latitude": 0,
    "longitude": 0
  }
]
```

On remarque que les données sont quasiment inutilisables, il nous manque au moins la date de chaque relevé de température. A ce stade les informations ne nous importe peu. 

On veut créer un endpoint qui renvoit des objets dto de type WeatherForecastDto. Et c'est fait !

Alors maintenant on va s'en assurer via les tests unitaires. 





