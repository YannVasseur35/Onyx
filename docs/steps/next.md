
Readme : expliquer l'apprentissage, qu'on fera des erreurs et qu'on les corrigera

- IAppServices

 
OperationResult

dataservice et guid IAudit

Guid : Jusque la, je n'avais pas besoin du Guid de l'objet en base de données (clé primaire). Je me disais meme que du point de vue sécurité, c'était pas plus mal. 
Vu que l'id est impossible à deviner (éviter les id interger exposer sur une api public), ce n'est pas possible d'accéder à des données à taton. 

Exemple concret : il fût une époque où, sur un site de rencontre à la sécurité douteuse, les photos étaient accessibles via un path du genre /api/users/UIOSOHF/photos/55. En voyant cela, on est tenté de taper 56 ou 54... et pourquoi pas faire une boucle de récupération d'images ? UIOSOHF étant l'id utilisateur, il suffit de récupérer celui d'un autre profil et on obtient un joli book photos. Et surprise, on s'apperçoit en plus que les photos qui ont été refusées par la modération ne sont pas effacées... Je vous laisse imaginer ce qu'est une photo refusée sur un site de rencontre. Vous voyez ce qu'un guid aurait apporté dans ce cas. Bien entendu on a d'autre moyen de sécuriser cela, mais c'est pas le sujet.

IAudit : 

            CreateMap<WeatherForecast, WeatherForecastEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) //Important, readonly
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) //Important, readonly







# LOGGer




Prise de note : prochain sujet à aborder 

# Clean DI

ajouter du code dans le projet Application et INfrastructure pour qu'il gère leurs propre injections, pour les re injecter en une ligne dans le Program.cs

# AutoFixture
https://autofixture.github.io/docs/quick-start/#


# Security

https://mwaseemzakir.substack.com/p/ep-33-how-can-you-avoid-common-security?r=m6y5h

secrets (password, passphrase, licence secret keys)

# CI / CD

une belle pipeline pour l'integration continue et de déploiement d'une appli