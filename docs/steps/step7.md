# Step 7 : ORM (Entity Framework)

Petit rappel. A ce stade on a :

- Une Api REST qui pour le moment répond que sur un seul endpoint qui nous donne des bulletins météos (tous)
- Une couche Application qui se charge de répondre à ce besoin, et qui en plus lance une notification en cas de gel.
- Une couche Infrastructure qui s'occupe de la persistance des données et pour le moment récupère toutes le données (en dur dans le code). 

La suite logique c'est de créer notre base de donnée, d'y ajouter un jeu de valeurs test et de retourner tout ça.

Une façon de faire est de passer par une commande SQL et créer nos propres requêtes SQL, s'assurer du mapping entre les colonnes des tables à notre objet (DAO). On faisait comme cela avant. Mais c'est beaucoup de boulot. 

On passe plutôt par un ORM qui va nous faciliter toutes ces tâches. Et dans notre cas l'ORM c'est Entity Framework. 


Mais avant d'en parler, on a un pti point à faire avant sur un Pattern bien connu qu'on met en général ici, juste avant les données de base de données. 

## Repository Pattern

Le patron Repository est utilisé pour abstaire la persistance ou la récupération de données, en général d'une base de données. Dit autrement, du point de vue des autres couches de l'application, en particulier la couche applicative, on a pas besoin de savoir comment sont stockées ou récupérées nos données, tout ce que l'on veut c'est un moyen de le faire. 

Ce moyen va être défini par une interface commune (située dans le Core et ainsi connue de tous)

### Définitions 

Vous allez voir des noms d'objets su style "Model", "Entities", "Dto", "Dao" ou que sais je encore. 

J'ai vu au cours de m'a carrière plusieurs définitions au sein de chaque équipe projet. Aussi, ces termes sont différents selon les languages de programation, l'expérience ou l'historique d'un projet. Au final, on s'y perds un peu.
- Model : terme super générique qui va définir nos objets. On retrouve se terme partout et à tous les niveaux ou couches. Trop vague et confusant. 
- DTO : Data Transfert Object, celui ci est plutôt clair et assez partagé par tous, c'est en gros un objet serialisable qui va finir en string JSON pour faire du transfert entre API et client, ou API et API. 
- DAO : Data Access Object. Objet proche d'une BD (qui recopie en général les colonnes d'une table). 
- Entity : terme aussi générique mais qui dans le monde .net a un sens si on utilise Entity Framework (EF), ce serait un DAO. Cependant en DDD (Domain Driven Devlopement) c'est un objet métier. Confusant tout ça.

(Je ne rentre pas dans le DDD ou on aurait aussi Aggregate, Value Object, etc...)

Dans ce projet, voila ce que moi j'entends 
- Model : Domain Model, ou Core. Objet métier de base. Proche de la logique métier. 
- DTO : Data Transfert Object 
- DAO : Pour m'a part je ne l'utilise pas, pour la raison qui suit.
- Entity : je vais utiliser EF qui nomme ces objets "Entity". Pour ne pas confondre, je vais garder le terme entity à ce niveau. 

Ces objets sont situées dans des couches différentes et vont devoir tranférer leurs données mutuellement. C'est le "mapping" qu'on a vu précédement.

D'ailleur EF est un ORM qui signifie "Object Relational Mapping". On les retrouvent devant une base de données et font l'intermédiaire avec notre code et la base de données. C'est super pratique mais pas forcément adapté à tous les projets. D'ailleurs est que le projet a besoin d'une base de données ?

### La version courte

Un Repository ou repo (à ne pas confondre avec un repo git) se résume en général à du CRUD (Create, Read, Update, Delete), le read est splité en deux avec un GetById et un GetAll(). Avec cela on couvre 95% des besoins pour tous les objets. 

Comme ce sont des actions de base d'un repo que quasi tous les "Data" vont adopter, on crée une interface générique que sera implémenté par une classe "service" qui fera le job.

Ainsi notre interface ressemblera à qq chose comme cela

```c#
public interface IRepository<T> 
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    void Insert(T entity);
    void Update(T entity);
    void Delete(T entity);
}
```

### Unit of Work Pattern

Une autre notion importante à comprendre lié au Repository Pattern c'est le Unit of Work.

Le pattern Unit of Work, selon Martin Fowler, maintient une liste d’objets affectés aux travers d’une transaction, il coordonne les modifications apportées à l’écriture et traite les éventuels problèmes de concurrence.

![](../images/UoW.png) 

Voyez le comme un chef d'orchestre qui gère à la fois les données de la base de données en lecture et écriture, ainsi qu'en mémoire une liste d'objets, ce qui pourrait s'apparenter à du cache quelque part. 


### Entity Framework fait il doublon ?

La question est, est ce que EF n'est pas déjà tout ça ? 
Le DbContext serait le Unit of Work et les DbSets les Repositories.

![](../images/Ef-UoW.png) 

Du coup, pourquoi rajouter une couche qui fait la même chose ?

Ce design pattern est sujet à contreverse avec l'utilisation Entity Framework, par exemple :

 - Generic Repository Pattern With EF Core - Why It Sucks : https://www.youtube.com/watch?v=Bz5JCbWnaHo

- Repository Pattern with C# and Entity Framework, Done Right | Mosh https://www.youtube.com/watch?v=rtXpYpZdOzM


### Pas de Save or Update dans le Repository

Si on écoute la vidéo précédente, on ne devrait pas avoir d'update ou de save dans notre interface IRepository (pour moi IDataService).

Je ne vois pas pourquoi, en tout cas pour l'Update. En effet le Save n'a pas de raison d'être ici, c'est la tambouille de l'infra et quand on utilise Add ou Update on sous entend "sauvegarde le en BD".

C'est ce "sous entend" qui fait la différence.

L'update serait donc séparée et accessible dans un UnitOfWork qui s'occupe de l'orchestration du Repository. 

### Comment je vois les choses

Pour rappel, on souhaite implémenter la couche de persistance des données, peut importe comment, avec ou sans EF, sur une base relationnelle ou un système de fichier, peu importe. Cela doit rester une abstraction. 

Cette abstraction est essentielle si on veut respecter la Clean Architecture. 
On ne doit pas dépendre d'un framework comme EF. C'est de la tambouille interne.

Et si on regarde bien IWeatherForecastDataServices c'est l'interface de mon repository WeatherForecast qui comporte à peu prêt les mêmes signatures. 
Mon Unit of work sera mon DBContext. On verra un jour si cela change...

PS : ya pas de code ici, c'est juste une page pour parler architecture/software design
