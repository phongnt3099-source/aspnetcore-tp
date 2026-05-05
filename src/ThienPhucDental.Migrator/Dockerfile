#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ThienPhucDental.Migrator/ThienPhucDental.Migrator.csproj", "src/ThienPhucDental.Migrator/"]
COPY ["src/ThienPhucDental.EntityFrameworkCore/ThienPhucDental.EntityFrameworkCore.csproj", "src/ThienPhucDental.EntityFrameworkCore/"]
COPY ["src/ThienPhucDental.Core/ThienPhucDental.Core.csproj", "src/ThienPhucDental.Core/"]
COPY ["src/ThienPhucDental.Core.Shared/ThienPhucDental.Core.Shared.csproj", "src/ThienPhucDental.Core.Shared/"]
RUN dotnet restore "src/ThienPhucDental.Migrator/ThienPhucDental.Migrator.csproj"
COPY . .
WORKDIR "/src/src/ThienPhucDental.Migrator"
RUN dotnet build "ThienPhucDental.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ThienPhucDental.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThienPhucDental.Migrator.dll", "-s"]
