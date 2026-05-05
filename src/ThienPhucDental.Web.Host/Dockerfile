#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ThienPhucDental.Web.Host/ThienPhucDental.Web.Host.csproj", "src/ThienPhucDental.Web.Host/"]
COPY ["src/ThienPhucDental.Web.Core/ThienPhucDental.Web.Core.csproj", "src/ThienPhucDental.Web.Core/"]
COPY ["src/ThienPhucDental.Application/ThienPhucDental.Application.csproj", "src/ThienPhucDental.Application/"]
COPY ["src/ThienPhucDental.Application.Shared/ThienPhucDental.Application.Shared.csproj", "src/ThienPhucDental.Application.Shared/"]
COPY ["src/ThienPhucDental.Core.Shared/ThienPhucDental.Core.Shared.csproj", "src/ThienPhucDental.Core.Shared/"]
COPY ["src/ThienPhucDental.Core/ThienPhucDental.Core.csproj", "src/ThienPhucDental.Core/"]
COPY ["src/ThienPhucDental.EntityFrameworkCore/ThienPhucDental.EntityFrameworkCore.csproj", "src/ThienPhucDental.EntityFrameworkCore/"]
COPY ["src/ThienPhucDental.GraphQL/ThienPhucDental.GraphQL.csproj", "src/ThienPhucDental.GraphQL/"]
RUN dotnet restore "src/ThienPhucDental.Web.Host/ThienPhucDental.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/ThienPhucDental.Web.Host"
RUN dotnet build "ThienPhucDental.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ThienPhucDental.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThienPhucDental.Web.Host.dll"]
