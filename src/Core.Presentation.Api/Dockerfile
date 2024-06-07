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
COPY ["Common/Core/src/Core.Presentation.Api/Core.Presentation.Api.csproj", "Common/Core/src/Core.Presentation.Api/"]
COPY ["Common/Core/src/Core.Domain/Core.Domain.csproj", "Common/Core/src/Core.Domain/"]
COPY ["Common/Core/src/Core.Application/Core.Application.csproj", "Common/Core/src/Core.Application/"]
COPY ["Common/Core/src/Core.Presentation/Core.Presentation.csproj", "Common/Core/src/Core.Presentation/"]
COPY ["Common/Core/src/Core.Infrastructure/Core.Infrastructure.csproj", "Common/Core/src/Core.Infrastructure/"]
COPY ["Common/Core/src/Core.Infrastructure.Persistence/Core.Infrastructure.Persistence.csproj", "Common/Core/src/Core.Infrastructure.Persistence/"]
RUN dotnet restore "./Common/Core/src/Core.Presentation.Api/./Core.Presentation.Api.csproj"
COPY . .
WORKDIR "/src/Common/Core/src/Core.Presentation.Api"
RUN dotnet build "./Core.Presentation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Core.Presentation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core.Presentation.Api.dll"]