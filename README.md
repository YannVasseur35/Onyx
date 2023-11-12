
![](docs/images/onyx.jpg)

# Onyx Solution 

Une solution Microsoft .net 7.0 / C# permettant l'apprentissage de concepts de software design, d'architecture, de clean code, de tests...

Permet de partir sur de bonnes bases pour un petit/moyen projet, basé sur quelques années d'expériences dans le software design. 

----

Onyx est une pierre précieuse de couleur noire. J'ai donnée ce nom à ce projet d'apprentissage car il est court, facilement identifiable, abstrait, et j'aime bien donner des "noms de code" à un projet. 

On va prendre comme exemple une application de météo (qui est d'ailleurs l'exemple par défaut d'une application .net web) on aura donc

- Une API qui expose des données
- une base de données SQLite avec EntityFramwork (code first)
- Une clean architecture avec de nombreux tests
- CI/CD avec une pipeline AzureDevops

## Introduction

On a tous commencé par écrire un seul projet c# et tout mettre dedans, sans vraiment rien structurer. Alors oui, ça marche et honnêtement vu les ambitions de cette app méteo c'est ce que l'on devrait faire. Et puis comme tout bon projet, il évolue, en quelques années tout un tas de nouvelles fonctionnalités se sont ajoutés, plusieurs devs sont passés dessus, avec du code plus ou moins clair. De plus avec le temps, l'infrastructure a changé, on a fait évoluer la base de données, certain services sont directement sur le cloud et on a du faire face à de la montée en charge. On a du "scaler" l'application, recoder des parties, ajouter du cache etc...Et bien entendu rien n'est testé automatiquement. Bref la vie d'une application dans pas mal de boite en France et ailleurs dans le monde. 

Je développe avec .net depuis plus de 20 ans. Et j'ai vu un certain nombre d'application (un bon paquet à vrai dire) finir dans ce que l'on appelle un "monolith legacy", très très dure à maintenir. Ces applications existent encore à l'heure ou j'écris ces lignes. Parfois on en est au deuxième projet de "refonte". Un cas typique est celui du projet ou l'on retrouve toute la logique métier dans les procédures SQL. Car c'est ce qui faisait vraiment gagner en pref à l'époque (et encore aujourd'hui). Mais seule la personne en charge de ces procédures stockées, souvent indéchiffrable et intestable, est en mesure de travailler dessus. Autre cas bancale, on a de la logique métier dans les pages web. Du coup on se retrouve à gérer à plusieurs endroits, du code qui fait plus ou moins la meme chose, au risque qu'un jour il ne fasse plus la même chose !
 
Heureusement aujourd'hui on a énormement plus de techniques, d'outils et de services qui permettent d'appréhender les projets autrement. Les entreprises comprennent les enjeux et les risques de garder trop longtemps du code legacy en production et s'engagent dans de gigantesque chantier de "renovation" voir même de ré-écriture complète. J'ai moi aussi contribuer à ces erreurs et je contribue aussi à leurs résolutions. 

Ce projet tente modestement de répondre à certaines problématiques couramment vues en entreprise. Je ne prétends pas tout résoudre mais apporter quelques réponses à des questions que moi même je me suis posé. Et à vrai dire, le meilleurs moyen d'y répondre pour moi est de tenter de le faire comprendre à quelqu'un d'autre. 

J'ai donc entrepris ce projet Onyx. 

## Progression

Vous avez deux façon de voir les choses :

- la rapide. Restez sur la branche master, la plus à jour et dernière en date. Vous avez tout le code source et tous les tests. Regardez le code et inspirez en vous. 

- l'apprentissage. Ce sera plus lent, mais j'explique tous les concepts pas à pas. On démarre gentillement et on progresse par étape (step) avec une général un concept. C'est une sorte de TP (travaux pratique) qu'on faisait à l'école. Pour suivre ce TP il faudra "naviguer" avec Git de branche en branche. C'est simple, toutes les branches s'appellent 'step' suvit d'un numéro. Chaque step aura sa propre doc (ex: step2.md). Suffit de s'y référer pour comprendre ce qu'y a été fait à chaque étape. 


## Sommaire

- [Step1 : Structure de solution](docs/steps/step1.md)
- [Step2 : Premier endpoint Api](docs/steps/step2.md)
- [Step3 : Test endpoint Api Rest](docs/steps/step3.md)
- [Step4 : Application Service et Tests](docs/steps/step4.md)
- [Step5 : DataService et Tests](docs/steps/step5.md)
- [Step6 : Mapping et Tests](docs/steps/step6.md)
- [Step7 : ORM (Entity Framework) et Tests](docs/steps/step7.md)
- [Step8 : Entity Framework](docs/steps/step8.md)
- [Step9 : Finalisation de la chaine de données](docs/steps/step9.md)
- [Step10 : Code Coverage ](docs/steps/step10.md)
- [Step11 : CRUD : Implémentation et Tests](docs/steps/step11.md)
- [Step12 : Tests d'intégration](docs/steps/step12.md)
- [Step13 : CI/CD AzureDevops Pipeline](docs/steps/step13.md)

## Git

Repo : https://dev.azure.com/ReactorLab/_git/Onyx

Vous devrez connaitre quelques commandes git de base très simple pour "naviguer" dans les branches de ce repo. 

Pour changer de branche :
```git
git checkout step1
```

Si vous avez fait des modifications et que vous souhaitez changer de branche, git va vous obliger à faire un commit ou à vous débarraser de ces changements. Comme vous ne contribuez pas au projet, débarrassez vous de vos changements avec la commande stash, puis faites votre checkout
```git
git stash
git checkout step2
```

## Documentations Annexes

Vous trouverez des informations complémentaires sur différents concept d'architecture ici (en anglais):

- [Main Architecture](docs/en/ARCHITECTURE.md)
- [Hexagonal Architecture](docs/en/HEXAGONAL.md)
- [DDD](docs/en/DDD.md)
- [CQRS AND ES](docs/en/CQRS-ES.md)
- [SOLID](docs/en/SOLID.md)

Tout est dans le répertoire docs.


