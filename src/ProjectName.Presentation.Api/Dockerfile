#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.Props", "."]
COPY ["src/ProjectName.Presentation.Api/ProjectName.Presentation.Api.csproj", "src/ProjectName.Presentation.Api/"]
COPY ["src/ProjectName.Domain/ProjectName.Domain.csproj", "src/ProjectName.Domain/"]
COPY ["src/ProjectName.Application/ProjectName.Application.csproj", "src/ProjectName.Application/"]
COPY ["src/ProjectName.Presentation/ProjectName.Presentation.csproj", "src/ProjectName.Presentation/"]
COPY ["src/ProjectName.Infrastructure/ProjectName.Infrastructure.csproj", "src/ProjectName.Infrastructure/"]
COPY ["src/ProjectName.Infrastructure.Persistence/ProjectName.Infrastructure.Persistence.csproj", "src/ProjectName.Infrastructure.Persistence/"]
RUN dotnet restore "./src/ProjectName.Presentation.Api/./ProjectName.Presentation.Api.csproj"
COPY . .
WORKDIR "/src/src/ProjectName.Presentation.Api"
RUN dotnet build "./ProjectName.Presentation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProjectName.Presentation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectName.Presentation.Api.dll"]