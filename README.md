
![](docs/images/onyx.jpg)

# Onyx Solution 

Une solution Microsoft .net 7.0 / C# permettant l'apprentissage de concepts d'architecture, de clean code, de tests...

Onyx est une pierre précieuse de couleur noire. J'ai donnée ce nom à ce projet d'apprentissage car il est court, facilement identifiable, abstrait, et j'aime bien donner des "noms de code" à un projet. 

On va prendre comme exemple une application de météo (qui est d'ailleurs l'exemple par défaut d'une application .net web) on aura donc

- Une API qui expose des données
- Une application web Blazor qui exploite ces données
- une base de données SQLite avec EntityFramwork (code first)

On pourrait facilement écrire un seul projet et tout mettre dedans. Oui, ça marche et honnetement vu le projet c'est ce que l'on devrait faire. Et puis comme tout bon projet, il évolue, en quelques années tout un tas de nouvelles fonctionnalités se sont ajoutés, plusieurs devs sont passés dessus, avec du code plus ou moins clair. De plus avec le temps, l'infrastructure a changé, on a fait évoluer la base de données, certain services sont directement sur le cloud et on a du faire face à de la montée en charge. On a du "scaler" l'application, recoder des parties, ajouter du cache etc...Et rien est testé automatiquement. Bref la vie d'une application. 

Je développe avec .net depuis plus de 20 ans. Et j'ai vu un certain nombre d'application (un bon paquet à vrai dire) finir dans ce que l'on appelle un monolith, du code legacy, très très dure à maintenir. Ces applications existent encore à l'heure ou j'écris ces lignes. Parfois on en est au deuxième projet de "refonte". Un cas typique est celui du projet ou l'on retrouve toute la logique métier dans les procédures SQL. Car c'est ce qui faisait vraiment gagner en pref à l'époque (et encore aujourd'hui). Mais seule la personne en charge de ces procédures stockées, souvent indéchiffrable et intestable, est en mesure de travailler dessus. Autre cas, on a de la logique metier dans les pages web. Du coup on se retrouve à gérer à plusieurs endroits du code qui fait plus ou moins la meme chose, au risque qu'un jour il ne fasse plus la même chose !
 
Heureusement aujourd'hui on a énormement plus de techniques, d'outils et de services en ligne, qui permettent de voir les projets autrement. Les entreprises comprennent les enjeux et les risques de garder trop longtemps du code legacy en production et s'engagent dans de gigantesque chantier de "renovation" voir même de ré-écriture complète. J'ai moi aussi contribuer à ces erreurs et je contribue aussi à leurs résolutions. 

Ce projet tente modestement de répondre à certaine problématique couramment vue en entreprise. Je n eprétends pas tout résoudre mais apporter quelques réponses à des questions que moi même je me suis posé. Et à vrai dire, le meilleurs moyen d'y répondre pour moi et de tenter de le faire comprendre à quelqu'un d'autre. 

J'ai donc entrepris ce chantier. 

## Progression

On va créer une application from scratch. Les étapes devraient être normalement toutes linéaires et se suivre. Ainsi vous pourrez grâce à Git "naviguer" dans la vie du projet assez facilement. De plus chaque step aura sa propre doc (ex: step2.md). Suffit de s'y référer pour
comprendre ce qu'y a été fait à chaque étape. 

## Sommaire

- [Step1 : Structure de solution](docs/steps/step1.md)
- [Step2 : Premier endpoint Api](docs/steps/step2.md)
- [Step3 : Test endpoint Api Rest](docs/steps/step3.md)
- [Step4 : Application Service](docs/steps/step4.md)


## Git

Repo : https://dev.azure.com/ReactorLab/_git/Onyx

Vous devrez connaitre quelques commandes git de base très simple pour "naviguer" dans les branches de ce repo. Voici ces branches (à ce stade)

- master : branch principale (pas utile pour vous dans le cadre d'un apprentissage)
- Step1 : projet de base (git checkout Step1)

Pour changer de branche :
```git
git checkout Step1
```

Si vous avez fait des modifications et que vous souhaitez changer de branche, git va vous obliger à faire un commit ou à vous débarraser de ces changements. Comme vous ne contribuez pas au projet, débarrassez vous de vos changements avec la commande stash, puis faites votre checkout
```git
git stash
```

## Documentations Annexes

Vous trouverez des informations complémentaires sur différents concept d'architecture ici (en anglais):

- [Main Architecture](docs/ARCHITECTURE.md)
- [Hexagonal Architecture](docs/HEXAGONAL.md)
- [DDD](docs/DDD.md)
- [CQRS AND ES](docs/CQRS-ES.md)
- [SOLID](docs/SOLID.md)

Tout est dans le répertoire docs.


