# Tests d'intégration

Nous y voila. Ces tests vont nous permettre d'être au plus proche des actions utilisateurs réélles. La seule chose que vous ne testerez pas ici c'est l'interface utilisateur (une page web ou une app mobile), mais plutôt l'api web (ou la couche application pour une app mobile ou desktop).
 
Le but est de simuler un parcours utilisateur afin de tester les cas les plus courants. On va appelez cela un scenario de test car il enchaine des étapes bien déterminées.

De plus, nous n'allons plus utiliser une base de données en mémoire mais bien une base de données réelle. Ceci va nous permettre deux choses:

- se confronter au réel et détecter de nouveau bug
- debuger plus facilemement en allant vérifier en base les données

# Nouveau Projet xUnit

Nous allons créer un 4ieme Projet xUnit Onyx.Web.Api.Integration.Tests

Celui ci sera très similaire à Onyx.Web.Api.Tests, cependant notre base de données sera réelle, pour cela on va commencer par changer notre AppTestWebApplicationFactory

```c#
    public class AppTestWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // remove the existing context configuration
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<OnyxDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                //SQLite Db Context
                services.AddDbContext<OnyxDbContext>(options => options.UseSqlite("DataSource=RubyDatabase.db"));
            });
        }
    }
```

Ensuite nous pouvons créer notre classe de test WeatherForecastsControllerTests

```c#
 [Collection("WeatherForecasts tests")]
 public class WeatherForecastsControllerTests : IClassFixture<AppTestWebApplicationFactory>
 {
    (...)
 }
```

Notez qu'on a rajouté un attribut "Collection". Ceci permet d'isoler nos tests pour qu'ils s'executent par groupe. Ainsi, on peut faire plusieurs classe de tests avec cette meme collection et executer l'ensemble. Dans notre cas ce n'est pas nécessaire. Je le mets la pour info car ça sert si vous avez plusieurs choses à tester indépendament.

Ici notre scenario est assez basique, nous allons effectuer dans l'ordre ces tâches :

- un simple get d'un bulletin météo qui n'existe pas encore, on s'attend à avoir une BadRequest
- une création d'un bulletin météo
- un simple get pour vérifier la création de ce dernier
- un update de ce bulletin météo
- un simple get pour vérifier ce dernier changement
- un delete pour supprimer définitivement ce bulletin météo
- un dernier get pour s'assurer que ce dernier n'existe plus. 

Ainsi on s'assure bu bon déroulé d'une durée de vie d'un objet bulletin météo. 

