FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER $APP_UID
WORKDIR /app
EXPOSE 8080
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Infrastructure/Identity.Api/Identity.Api.csproj", "Infrastructure/Identity.Api/"]
RUN dotnet restore "Infrastructure/Identity.Api/Identity.Api.csproj"
COPY . .
WORKDIR "/src/Infrastructure/Identity.Api"
RUN dotnet build "Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecAll.Infrastructure.Identity.Api.dll"]
