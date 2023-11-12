# CI/CD AzureDevops Pipeline

Un point que j'affectionne particulièrement c'est le devops. En réalité, tout ce que l'on vient de faire jusque la prend réellement sens lorsque l'on monte une chaine d'intégration et de déploiement automatique (Continiuous Integration, Continiuous Deployment).

Jusque là, on travaillait en local et avec une team de 1 dev. Nous même. 

Imagininons maintenant que la team évolue et intègre deux nouveaux développeurs juniors. Je le vois dans vos yeux, vous avez peur pour votre code. Et je vous donne pas tord. 

Les juniors (et nous même) allons faire des erreurs dans le code et les tests unitaire et d'intégration nous permettent déjà une certaine sécurité. Seulement faut les executer régulièrement. On y pense pas forcément. 

Il serait intéressant d'automatiser cela, dans un context "neutre".

## Intégration continue

L'intégration continue est le terme utilisé pour ajouter un bout de code au code source principal (master ou main). On utilise maintenant tous git, donc parlons d'un repo git.

Si on travaille seul, on fait ça tranquille en local, sur la branche master, puis on push sur le repo git. 

Maintenant si on travaille à plusieurs, ba on fait la même chose. La différence c'est que l'on doit intégrer localement le travail des autres devs qui ont mis à jour le repo distant entre temps. On fait un Merge ou un Rebase et on croise les doigts pour qu'il n'y ait aucun conflit à résoudre. Ensuite avant de pousser sur le repo distant nos modifications, on lance les tests. Si c'est tout vert on pousse. 

Voilà ça c'est la théorie. 

Dans la pratique, vos juniors vont vous pourrir votre branche master. Y aura plein de conflits à gérer et faudra passer du temps à faire le ménage. C'est pas du dévéloppement, c'est du temps perdu.

Pour résoudre ce genre de problème on a plusieurs solutions. Je vous propose la mienne. Il n'y a pas une seule solution ou manière de faire. Cela va dépendre de la taille de votre équipe et de la façon dont vous gérez vos livrables. 

En gros on a :
- un environnement de production qui va recevoir du code provenant de la branche master
- un environnement de staging (recette) qui va recevoir du code provenant de la branche develop

A chaque modification dans la branche develop qui est poussé sur repo distant, cela lance automatiquement une pipeline Azure Devops. Idem pour master.

La pipeline de develop va lancer un build, puis lancer tous les tests et si ca passe jusque la, on lance un deploiement automatique vers notre environement staging. Ainsi, on met automatiquement à jour l'environement Staging à chaque fois qu'on pousse du code sur notre branche develop. 

Idem pour master. Dès qu'elle reçoit un nouveau commit, elle build puis déploie automatiquement en prod. Notez qu'on ne test pas le code ici. On pourrait, mais ce n'est pas nécessaire si on bloque la branche master à toute modification en dehors d'Azure Devops.

## Déploiement continue

Le déploiement continue permet de livrer votre application à vos clients. Ca peut être un application desktop avec installation déposé dans un FTP, ou une application mobile déposé sur les stores iOS/Android ou un site web sur un VPS, ou AWS ou Azure. 

Bref, c'est votre tambouille. Dans notre cas, c'est une Api web qu'on déposera via SSH sur un VPS fictif. 


## Branche git

Je vous conseille d'aller jetter un oeil sur Git Flow : https://danielkummer.github.io/git-flow-cheatsheet/index.fr_FR.html

Bloquer la branche master à toute modification des devs peut paraitre absurde, mais vous allez voir qu'avoir une branche propre, à jour, qui compile, et marche, c'est une très bonne chose. Vous trouverez comment faire dans cet article. https://reactor.fr/azure-devops-pull-request/

Il faut alors créer une nouvelle branche "develop" qui va nous servir de branche principale de travail. 

Maintenant à vous de voir, vous pouvez tous bosser sur la branche develop et merger votre code quand il faut. Ou vous pouvez créer une branche 'feature', 'fix' ou autre et rebaser votre code, puis pousser cette branche sur le repo distant. Enfin, avec Azure Devops vous demander une Pull Request pour l'intégrer dans votre branche dev. 

On a pleins de façon de faire. Je pense qu'il faut être pragmatique ici. Si l'équipe n'est pas à l'aise avec git ou plein de branche, si l'équipe est petite ou que chacun bosse sur une partie bien distinct, ce n'est peut être pas la peine de pousser le git-flow jusqu'au boutisme. 

## Pipeline CI

Onyx n'aipas de branche develop. Nous allons créer une seule pipeline pour l'exemple, car dans ce projet je n'ai ni staging, ni prod. Elle se trouve dans le dossier 'devops' à la racine.

Une pipeline (gros tuyaux) est une succession de tâche listées dans un fichier .yaml. C'est un fichier dont la structure est "normée", par exemple l'indentation est importante. Une pipeline va donc donner une suite d'instruction à effectuer. Si une échoue, la pipeline s'arrête et ne poursuit pas.

Nous allons faire ceci dans cet ordre :

```yaml
trigger:
- master
```
- définir un trigger (pour nous master, pour la branche master). Cette pipeline sera donc lancé sur chaque nouveau commit de la branche master.

```yaml
pool:
  vmImage: ubuntu-latest
```
- pool vmImage : on défini sur quel OS virtuel on va executer notre pipeline. Si vous avez du .net Framework 4.6 une image Windows sera nécessaire. Nous en .net7 on peut utiliser une plateforme linux.
  
```yaml
variables:
  dotnetSdkVersion: '7.x'
  projectPath: 'src/Onux.Web.Api/Onyx.Web.Api.csproj' 
  buildConfiguration: 'Release'
  dateToday: 'Will be set dynamically'
  revision: $[counter(format('{0:dd}', pipeline.startTime), 1)]  
  versionString: > 
    $(dateToday).$(revision)
steps:
```
- variables : on définit un ensemble de variable qui vont être réutiliser dans le fichier.
  
```yaml
- task: UseDotNet@2
  displayName: 'Use .NET SDK $(dotnetSdkVersion)'
  inputs:
    version: '$(dotnetSdkVersion)'
```
- definition de la version de .net que l'on souhaite utiliser
  
```yaml
- task: NuGetToolInstaller@1
  inputs:
    versionSpec: 6.4.0 
```
- on install nuget
  
```yaml
- script: 'echo "$(Build.DefinitionName), $(Build.BuildId), $(Build.BuildNumber)" > buildinfo.txt'
  displayName: 'Write build info'
  workingDirectory: $(Agent.BuildDirectory)
```
- on affiche une version de build (permet de suivre dans Azure Devops ce qu'on fait)
  
```yaml
- task: DotNetCoreCLI@2
  displayName: 'Build the project - $(buildConfiguration)'
  inputs:
    command: 'build'
    arguments: '--configuration $(buildConfiguration)'
    projects: '$(projectPath)'
```
- on build notre code source (de la branche master)
  
```yaml
- task: DotNetCoreCLI@2
  displayName: 'Run unit tests - $(buildConfiguration)'
  inputs:
    command: 'test'
    arguments: '--configuration $(buildConfiguration)'
    publishTestResults: true
    projects: '**/*.Tests.csproj'

```
- on lance les tests (tous les projets terminant par Tests.csproj)
  
```yaml
- task: PowerShell@2
  displayName: 'Preparing Build Number'
  inputs:
    targetType: 'inline'
    script: |
      $currentDate = $(Get-Date)
      $year = $currentDate.Year
      $month = $currentDate.Month
      $day = $currentDate.Day
      Write-Host $currentDate
      Write-Host $day
      Write-Host $env:revision
      Write-Host "##vso[task.setvariable variable=dateToday]$year.$month.$day"
```
- on construit un numéro de build
  
```yaml
- script: echo $(versionString)
  displayName: 'versionString display'
```
- qu'on affiche (permet de suivre dans Azure Devops ce qu'on fait)
  
```yaml
- task: DotNetCoreCLI@2
  displayName: Publish
  inputs:
    command: 'publish'
    publishWebProjects: false
    projects: '$(projectPath)'
    arguments: '--configuration $(buildConfiguration) --output $(build.artifactdirectory) /p:Version=$(versionString)'
    zipAfterPublish: false
    modifyOutputPath: false
```
- et enfin on upload le tout via SSH sur un serveur distant. 
  
```yaml
- task: CopyFilesOverSSH@0
  displayName: 'Uploading files SFTP'
  inputs:
    sshEndpoint: 'SSH connexion'
    sourceFolder: $(build.artifactdirectory)
    targetFolder: '/var/www/api.meteo.fr' 
    cleanTargetFolder: true 

```

Pour cette dernière étape, le serveur distant n'existe pas. 'SSH connexion' est le nom (fictif) d'une connexion SSH enregistrée sur Azure Devops. Si vous voulez en savoir plus : https://reactor.fr/azure-devops-pipeline-ci-cd-deploy/

Il ne vous reste plus qu'à créer une pipeline sur Azure Devops en prenant soins de séléctionner ce fichier yaml.

Pour cela soit vous le créez en ligne, soit vous le commit/push sur la branche master. (qui à ce moment la ne doit pas être bloquée ;) )

Notez qu'il va vous falloir faire un peu de gymnastique intellectuelle lorsque vous allez jouer avec plusieurs branche (master et develop par exemple) qui auront des triggers sur celle ci. Or si vous uploader une modification du fichier yaml sur la branche develop, elle ne sera prise en compte que lorsque du merge futur(PR) vers master. Cependant Azure Devops vous permet de choisir une branche manuellement lors d'un run manuel d'une pipeline.

# Conclusion :

Azure Devops regorge d'outils pour les développeurs et les 'ops'. Utiliser des pipelines pour alimenter une CI/CD permet d'assurer une excellente qualité de code et une livraison de vos travaux automatique sans prise de tête.

Couplé à des tests, vous avez un combo gagnant pour votre équipe. 










