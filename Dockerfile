FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5111
EXPOSE 7133

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TABP.Api/TABP.Api.csproj", "TABP.Api/"]
COPY ["TABP.Application/TABP.Application.csproj", "TABP.Application/"]
COPY ["TABP.DA/TABP.DA.csproj", "TABP.DA/"]
COPY ["TABP.Domain/TABP.Domain.csproj", "TABP.Domain/"]
RUN dotnet restore "TABP.Api/TABP.Api.csproj"
COPY . .
WORKDIR "/src/TABP.Api"
RUN dotnet build "TABP.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TABP.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TABP.Api.dll"]
