
![](docs/images/onyx.jpg)

# Onyx Solution 

Une solution Microsoft .net 7.0 / C# permettant l'apprentissage de concepts d'architecture, de clean code, de tests...

Onyx est une pierre précieuse de couleur noire. J'ai donnée ce nom à ce projet d'apprentissage car il est court, facilement identifiable, abstrait, et j'aime bien donner des "noms de code" à un projet. 

## Sommaire

[Step1 : structure de solution](docs/steps/step1.md)


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


