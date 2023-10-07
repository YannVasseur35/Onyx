# Step 1 : Mise en place de la solution

![](../images/solution-step1.jpg)

## Structure globale

- 1-solution : répertoire listant les fichiers racines du projet (ex .gitignore)
- src : répertoire contenant tout le code source de nos applications
- tests : répertoire contenant tous les projets de tests concernant les projets de src


## Onion Architecture

On va avoir besoin de comprendre quelques conception d'archicture en "oignon" (l'image est celle des couches d'un oignon)

Tapez onion architecture dans votre moteur de recherche, section image. Vous allez tomber sur de nombreuses représentations des "couches" d'un oignon. Elles se ressemblent beaucoup, vous allez voir des couches nommées  "Infrastructure", "Application", "Domain", "Core". Cependant vous remarquerez quelques variantes avec des termes différents. 

![](../images/onion-arch3.png)
<!-- ![](../images/onion_architecture.webp) ![](../images/onion-arch2.jpg) ![](../images/onion-arch1.png) -->

La seule règle c'est que toute les dépendances de vos projets vont vers le centre (core). Et que le centre (core) ne dépend de rien. C'est très important et c'est ce qui différencie cette achitecture à une architecture "N-Tiers" (Presentation Layer - Business Layer - Data Access Layer) où la couche business a une dépendance à la couche data access. 

Dans une architecture oignon, cette dépendance n'existe pas. C'est la couche database qui a une dépendance à une couche inférieur (core). On utilise alors une interface (définie dans la couche core) implémentée dans la couche database. On obtient ainsi une séparation forte entre la logique metier et son infrastructure. 

Cette séparation permet d'ajouter facilement des tests sur chaque couche (projet .net), indépendament des autres. Ceci permet aussi le remplacement d'une couche externe par une autre. Imaginez par exemple qune couche logging gérer uniquement via la console. Un jour on décide passer sur un système de gestion online. Il faudra alors implémenter un nouveau service basé sur une même interface.

Autrement dit, on ne change pas la couche métier. Elle donne toujours des ordres comme "log moi ca", après c'est la couche infrastructure qui execute la demande. Comment ? La couche métier s'en contre fou et c'est pas son problème.


## Répertoire src

C'est une pratique commune que l'on retrouve dans quasiment tous les projets de développement. Src signifie source, autrement dit, notre code source. 

Ce répertoire est composé de 4 projets .net, dont un web. A ce stade ils sont tous vide. Cependant le projet web peut s'executer et affichera un Swagger vide. 

- **Onyx.Core** est le projet métier qu'on aurait pu appeler aussi "domain", est au centre de notre architecture. Il n'a aucune dépendance aux couches supérieures mais expose des interfaces pour les "utiliser". Il contient nos objets métiers. Il va aussi contenir nos services metiers (domain) qu'on aurait pu mettre dans un projet à part. Restons simple. 
- **Onyx.Application** consitue la couche intermediaire entre le Core et le reste, comme une API Web ou une app mobile par exemple.
- **Onyx.Infrastructure** c'est ce projet qui va gérer une base de donnée, un système de fichier, un service web etc. Imaginez que la couche application demande un "Save" d'une donnée, la couche infrastructure s'en occupe. Peut importe comment, sur une base de données, dans un cloud, la couche application n'a pas à le savoir.
- **Onyx.Web.Api** c'est notre projet de "présentation" (ca aurait pu être un site web ou une app mobile). Il va utiliser les services de la couche application pour exposer des endpoints accessibles depuis le web. 


## Répertoire test

Ce répertoire parle de lui même, on y trouve un projet test par projet src. Ce sont nos tests unitaires. On verra plus tard que l'on aura d'autre type de projet tests.

