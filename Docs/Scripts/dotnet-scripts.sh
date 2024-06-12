
dotnet ef migrations add AuditingEntityMigrations -s src/Core.Presentation.Api/ -p src/Core.Infrastructure.Persistence/ -o Migrations/SqlServer
dotnet ef migrations add InitialMigrations -s src/Core.Presentation.Api/ -p src/Core.Infrastructure.Persistence/ -o Migrations/Postgres

dotnet ef database update -s src/Core.Presentation.Api/ -p src/Core.Infrastructure.Persistence/

