dotnet ef migrations add InitialMigrations -s src/ProjectName.Presentation.Api/ -p src/ProjectName.Infrastructure.Persistence.SqlServer/ -c ProjectName.Infrastructure.Persistence.ApplicationDbContext;
dotnet ef database update -s src/ProjectName.Presentation.Api/ -p src/ProjectName.Infrastructure.Persistence.SqlServer/

dotnet ef migrations add InitialMigrations -s src/ProjectName.Presentation.Api/ -p src/ProjectName.Infrastructure.Persistence.Postgres/ -c ProjectName.Infrastructure.Persistence.ApplicationDbContext;
dotnet ef database update -s src/ProjectName.Presentation.Api/ -p src/ProjectName.Infrastructure.Persistence.Postgres/


dotnet ef migrations add MessagesMigrations -s src/ProjectName.Presentation.Api/ -p src/ProjectName.Infrastructure.Persistence.SqlServer/ -c ProjectName.Infrastructure.Persistence.ApplicationDbContext;


