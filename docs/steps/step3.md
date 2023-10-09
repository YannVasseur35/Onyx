# Step 3 : Test endpoint Api Rest

On vient de cr�er notre premier endpoint. Comment on test une API ? Et que test-on ?

On va faire des tests unitaires tr�s basique. On verra les tests d'integrations (ou tests fonctionnels) dans une autre �tape.

Ici on ne va pas tester les donn�es, l'id�e c'est de tester l'API Web et son comportement, notamment des erreurs suivantes :

- 200 : Success, code qui indique OK, donc que tout va bien. 
- 400 : Bad Request, � l'inverse, celui ci inque un probl�me (toutes les erreurs 40x indique un probl�me), bad request est utilis� lorsque l'on passe des param�tres en entr�e, par exemple un id. Si celui ci est pas pr�sent ou mal form�, on revoit une erreur Bad Request 400
- 401 : Unauthorized, l'utilisateur qui fait la requ�te web n'est pas authentifi�.
- 403 : Forbidden, l'utilisateur qui fait la requ�te web est authentifi� mais ne dispose pas des droits pour acc�der � cette resource.

Il y a d'autres retours int�ressant � tester (201, 301, 302 ...) mais ceux lister ci dessus sont les principaux.

Dans notre exemple, l'acc�s est libre � tous sans authentification ni autorisation (anonymous). On aura pas � tester la 401 et la 403.

Je vais vous pr�senter deux fa�ons de tester son code. La premi�re est la plus simple � mettre en place mais n�cessite de lancer manuellement son server local wep api. La seconde a le m�rite d'�muler un serveur web ce qui permet de lancer son test en mode autonome.

Le projet de test Onyx.Web.Api.Tests a une r�f�rence vers le projet Onyx.Application.
C'est un projet utilisant la lib xUnit.


## M�thode 1 : Client Http classique

On applique la r�gle des "3 A": 
- Arrange : pr�paration des donn�es de test
- Act : appel de la m�thode � tester
- Assert : contr�le des valeurs attendues 

Dans arrange, on ajouter les m�mes donn�es issue du controleur. C'est pour que le test passe. Pour faire du TDD, la d�marche serait de lancer un test en �chec, puis de le faire passer. On a ici cette derni�re �tape. 

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

Notez que vous aurez besoin de lancer votre Api Web (pour moi sur https://localhost:7006) avant de lancer ce test. Si tout va bien, vous aurez un test en succ�s.

Conclusion : on peut tester une api en utilisant directement un client Http. Mais ce n'est pas pratique, il faut lancer manuellement une instance de l'api. Autrement dit, si vous n'avez pas lancer un serveur web api, les tests vont �chouer. Ceci s'av�rera encore plus probl�matique par la suite si l'on souhaite lancer nos tests dans une pipeline sur azuredevops par exemple. Dans un contexte CI/CD vous allez casser une integration (Pull Request) ou un d�ploiement. 


##  M�thode 2 : IClassFixture<WebApplicationFactory<Program>>

Doc officielle : Test d'integration en ASP.NET Core 7.0 : 
https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0

Nous allons cr�er une instance "test web host" autonome. Pour cela nous allons avoir besoin du package suivant :

```
Microsoft.AspNetCore.Mvc.Testing
```

Le package Microsoft.AspNetCore.Mvc.Testing inclue une classe WebApplicationFactory<TEntryPoint> qui cr�e une instance de TestServer, qui ex�cute l'API en m�moire lors d'un test. C'est pratique car nous n'avons pas besoin que l'API soit r�ellement en cours d'ex�cution avant de lancer ces tests d'int�gration. 

Cr�eons une seconde classe WeatherForecastsControllerTestsV2 (qui disparaitra dans les prochaines �tapes, afin de garder les deux exemples dans cette branche git)

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

 \<Program> fait r�f�rence � la classe Program de Onyx.Web.Api, m�me si j'ai pas bien compris comment cette relation est faite. 
 
 Toujours est il qu'il vous faudra une r�f�rence de Onyx.Web.Api dans votre projet Onyx.Web.Api.Tests. De plus il faudra rajouter cette ligne � cette classe Program.cs afin qu'elle soit accessible depuis le projet test. 
 J'ai enlev� la r�f�rence au projet Onyx.Application car celui ci est implicitement r�f�renc� par Onyx.Web.Api

 ```c#
 // Make the implicit Program class public so test projects can access it
public partial class Program
{ }
```

Maintenant vous pouvez executer ce test, plus besoin de lancer manuellement un serveur web api.












