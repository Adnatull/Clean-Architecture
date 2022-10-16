# Clean-Architecture
Clean Architecture in ASP.Net 6.0. This contains  Onion/Hexagonal architecture, DDD, CQRS using mediaTr, Unit Testing, Functional Testing, ASP.NET Core Identity, Entity Framework Core - Code First, Linq2db, Repository Pattern - Generic, Swagger UI, Response Wrappers,  API Versioning, Automapper, Serilog, Exception handling, and so on.

# Docker
If you want to run the application using docker-compose, execute below commands
```console
docker-compose build
docker-compose up
```


## Default User Credentials
UserName: SuperAdmin\
Email: a2masum@yahoo.com\
Password: SuperAdmin

Or You can change them from **Core.Domain.Identity.Constants.DefaultApplicationUsers.cs & Infrastructure.Identity.Seeds.IdentityMigrationManager.cs**
