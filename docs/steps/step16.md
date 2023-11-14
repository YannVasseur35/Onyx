# Sécurisation des données sensibles

Dans l'étape précédente, on a stocké dans le fichier appsettings.json ces informations :

```json
{
  "OpenWeatherMapApiBaseUrl": "https://api.openweathermap.org/data/2.5/weather",
  "OpenWeatherMapApiKey": "2ab3de3a263b8b265877be0c4acf25b3",
  (...)
}
```

OpenWeatherMapApiBaseUrl est une donnée publique non sensible. Par contre OpenWeatherMapApiKey est une donnée privée sensible. Cette clé est lié à un compte spécifique qui est susceptible d'être facturé. 

Un vol de cette clé permettrait à une personne mal intentionné de se servir de l'API tierce à la place du propriétaire. De plus si l'api tierce (c'est pas vraiment le cas avec OWM) dispose de données utilisateurs on s'expose à d'autre vol de données (usurpation d'identité, numéro de carte de crédit, numéro de tel pour spam, email pour spam...).

La plupart des Apis permettent de renouveler ces clés. Et c'est une bonne pratique de les changer régulièrement. 
Par exemple un ancien employé pourrait avoir copié ces clés pour son usage personnel. Il serait bon de renouveler celles ci.

De plus, le gestionnaire de code source (git pour la majorité des cas) va stocker dans on repo cette clé. Elle sera donc connu de tout ceux qui ont accès au repo, c'est à dire tous les développeurs, les devops, du management et autre. 

Pourquoi exposer ces données sensibles ainsi ? 

La flemme en premier lieux. C'est vrai que si vous avez un répo privée avec peu de personnel, ce n'est pas nécessaire de prendre plus de mesure que cela. Personnellement je n'applique pas ce qui va suivre car je suis seul sur mes projets. De plus pour les projets en équipe dans un contexte professionnel, je n'ai pas encore vu cela s'appliquer. 

Il faut noter aussi qu'appliquer ce qui suit rend la gestion des environnements staging et production plus compliqué.

La sécurité a un coût. Elle ajoute de la complexité voir demande des services payants supplémentaires comme Azure Key Vault ou AWS Secrets Manager.

## Secret Manager

L'idée c'est de retirer la clé "OpenWeatherMapApiKey": "2ab3de3a263b8b265877be0c4acf25b3" pour que nulle part dans le code on est accès à celle ci. Ainsi le repo git n'aura aucune clé. Il pourra alors être publique en toute sécurité. 

Mias où stocker cette clé ? 

Microsoft répond à cette question avec un gestionnaire de secret :
https://learn.microsoft.com/fr-fr/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows#secret-manager

Pour la faire courte, un fichier de config comme appsettings.json va être créer dans votre espace personnel dans AppData :

```
%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json
```

Lors d'un run de votre programme local sur la machine de developpement, ce fichier sera automatiquement chargé dans la configuration de votre programme. Mettez un breakpoint juste après la création d'un builder :

```
var builder = WebApplication.CreateBuilder(args);

<breakpoint>
```














