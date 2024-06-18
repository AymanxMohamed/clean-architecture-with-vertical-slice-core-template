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
COPY ["src/Core.Presentation.Api/Core.Presentation.Api.csproj", "src/Core.Presentation.Api/"]
COPY ["src/Core.Domain/Core.Domain.csproj", "src/Core.Domain/"]
COPY ["src/Core.Application/Core.Application.csproj", "src/Core.Application/"]
COPY ["src/Core.Presentation/Core.Presentation.csproj", "src/Core.Presentation/"]
COPY ["src/Core.Infrastructure/Core.Infrastructure.csproj", "src/Core.Infrastructure/"]
COPY ["src/Core.Infrastructure.Persistence/Core.Infrastructure.Persistence.csproj", "src/Core.Infrastructure.Persistence/"]
RUN dotnet restore "./src/Core.Presentation.Api/./Core.Presentation.Api.csproj"
COPY . .
WORKDIR "/src/src/Core.Presentation.Api"
RUN dotnet build "./Core.Presentation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Core.Presentation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core.Presentation.Api.dll"]