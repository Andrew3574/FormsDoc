# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5873
ENV ASPNETCORE_URLS=http://:+5873
ENV FormsApi__apiUrl=http://46.21.247.63:8080/api/


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development 
WORKDIR /src
COPY ["FormsAPP/FormsAPP.csproj", "FormsAPP/"]
RUN dotnet restore "./FormsAPP/FormsAPP.csproj"
COPY . .
WORKDIR "/src/FormsAPP"
RUN dotnet build "./FormsAPP.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Development 
RUN dotnet publish "./FormsAPP.csproj" -c $BUILD_CONFIGURATION -o /app/publish 

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FormsAPP.dll"]