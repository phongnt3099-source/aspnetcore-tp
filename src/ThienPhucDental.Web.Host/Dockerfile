# Sửa từ 7.0 thành 8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Sửa từ 7.0 thành 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy toàn bộ mã nguồn bao gồm file .sln và các file config
COPY . .

# Restore trực tiếp toàn bộ Solution thay vì từng project lẻ
RUN dotnet restore "ThienPhucDental.Web.sln" --disable-parallel

# Tiến hành Build và Publish
WORKDIR "/src/src/ThienPhucDental.Web.Host"
RUN dotnet build "ThienPhucDental.Web.Host.csproj" -c Release -o /app/build
RUN dotnet publish "ThienPhucDental.Web.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ThienPhucDental.Web.Host.dll"]