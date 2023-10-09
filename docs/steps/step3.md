# Step 3 : Test endpoint Api Rest

On vient de créer notre premier endpoint. Comment on test une API ? Et que test-on ?

On va faire des tests unitaires très basique. On verra les tests d'integrations (ou tests fonctionnels) dans une autre étape.

Ici on ne va pas tester les données, l'idée c'est de tester l'API Web et son comportement, notamment des erreurs suivantes :

- 200 : Success, code qui indique OK, donc que tout va bien. 
- 400 : Bad Request, à l'inverse, celui ci inque un problème (toutes les erreurs 40x indique un problème), bad request est utilisé lorsque l'on passe des paramètres en entrée, par exemple un id. Si celui ci est pas présent ou mal formé, on revoit une erreur Bad Request 400
- 401 : Unauthorized, l'utilisateur qui fait la requête web n'est pas authentifié.
- 403 : Forbidden, l'utilisateur qui fait la requête web est authentifié mais ne dispose pas des droits pour accéder à cette resource.

Il y a d'autres retours intéressant à tester (201, 301, 302 ...) mais ceux lister ci dessus sont les principaux.

Dans notre exemple, l'accès est libre à tous sans authentification ni autorisation (anonymous). On aura pas à tester la 401 et la 403.

Je vais vous présenter deux façons de tester son code. La première est la plus simple à mettre en place mais nécessite de lancer manuellement son server local wep api. La seconde a le mérite d'émuler un serveur web ce qui permet de lancer son test en mode autonome.

Le projet de test Onyx.Web.Api.Tests a une référence vers le projet Onyx.Application.
C'est un projet utilisant la lib xUnit.


## Méthode 1 : Client Http classique

On applique la règle des "3 A": 
- Arrange : préparation des données de test
- Act : appel de la méthode à tester
- Assert : contrôle des valeurs attendues 

Dans arrange, on ajouter les mêmes données issue du controleur. C'est pour que le test passe. Pour faire du TDD, la démarche serait de lancer un test en échec, puis de le faire passer. On a ici cette dernière étape. 

```c#
using Onyx.Application.Dtos;
using System;
using System.Collections.Generic;

namespace Onyx.Web.Api.Tests.Contollers
{
    public class WeatherForecastsControllerTests
    {
        private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("https://localhost:7006") };
        private const string baseEndPoint = "/api/playerjourneys";
               
         [Fact]
        public async Task GetAllWeatherForecasts_ShouldReturn_Ok()
        {
            //Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
 
            //Act
            var response = await _httpClient.GetAsync($"{baseEndPoint}");

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode); 
        }
    }
}
```

Notez que vous aurez besoin de lancer votre Api Web (pour moi sur https://localhost:7006) avant de lancer ce test. Si tout va bien, vous aurez un test en succès.

Conclusion : on peut tester une api en utilisant directement un client Http. Mais ce n'est pas pratique, il faut lancer manuellement une instance de l'api. Autrement dit, si vous n'avez pas lancer un serveur web api, les tests vont échouer. Ceci s'avérera encore plus problématique par la suite si l'on souhaite lancer nos tests dans une pipeline sur azuredevops par exemple. Dans un contexte CI/CD vous allez casser une integration (Pull Request) ou un déploiement. 


##  Méthode 2 : IClassFixture<WebApplicationFactory<Program>>

Doc officielle : Test d'integration en ASP.NET Core 7.0 : 
https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0

Nous allons créer une instance "test web host" autonome. Pour cela nous allons avoir besoin du package suivant :

```
Microsoft.AspNetCore.Mvc.Testing
```

Le package Microsoft.AspNetCore.Mvc.Testing inclue une classe WebApplicationFactory<TEntryPoint> qui crée une instance de TestServer, qui exécute l'API en mémoire lors d'un test. C'est pratique car nous n'avons pas besoin que l'API soit réellement en cours d'exécution avant de lancer ces tests d'intégration. 

Créeons une seconde classe WeatherForecastsControllerTestsV2 (qui disparaitra dans les prochaines étapes, afin de garder les deux exemples dans cette branche git)

On va utiliser IClassFixture https://xunit.net/docs/shared-context qui permet de partager un context d'execution parmis tous les tests d'une classe de test.

```c#
namespace Onyx.Web.Api.Tests.Contollers
{
    public class WeatherForecastsControllerTestsV2 : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string baseEndPoint = "/api/weatherforecasts";

        private readonly WebApplicationFactory<Program> _factory;

        public WeatherForecastsControllerTestsV2(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllWeatherForecasts_ShouldReturn_Ok()
        {
            using var _httpClient = _factory.CreateClient();

            //Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;

            //Act
            var response = await _httpClient.GetAsync($"{baseEndPoint}");

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
    }
}
```

 Notez que l'on injecte via la contructeur une instance de WebApplicationFactory. Ainsi, on pourra l'utiliser dans tous nos tests futurs de cette classe. 

 \<Program> fait référence à la classe Program de Onyx.Web.Api, même si j'ai pas bien compris comment cette relation est faite. 
 
 Toujours est il qu'il vous faudra une référence de Onyx.Web.Api dans votre projet Onyx.Web.Api.Tests. De plus il faudra rajouter cette ligne à cette classe Program.cs afin qu'elle soit accessible depuis le projet test. 
 J'ai enlevé la référence au projet Onyx.Application car celui ci est implicitement référencé par Onyx.Web.Api

 ```c#
 // Make the implicit Program class public so test projects can access it
public partial class Program
{ }
```

Maintenant vous pouvez executer ce test, plus besoin de lancer manuellement un serveur web api.












