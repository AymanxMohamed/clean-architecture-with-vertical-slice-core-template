dotnet ef migrations add InitialMigrations -s src/Core.Presentation.Api/ -p src/Core.Infrastructure.Persistence.SqlServer/ -c Core.Infrastructure.Persistence.ApplicationDbContext;
dotnet ef database update -s src/Core.Presentation.Api/ -p src/Core.Infrastructure.Persistence.SqlServer/

dotnet ef migrations add InitialMigrations -s src/Core.Presentation.Api/ -p src/Core.Infrastructure.Persistence.Postgres/ -c Core.Infrastructure.Persistence.ApplicationDbContext;
dotnet ef database update -s src/Core.Presentation.Api/ -p src/Core.Infrastructure.Persistence.Postgres/


dotnet ef migrations add MessagesMigrations -s src/Core.Presentation.Api/ -p src/Core.Infrastructure.Persistence.SqlServer/ -c Core.Infrastructure.Persistence.ApplicationDbContext;


