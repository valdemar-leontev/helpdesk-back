FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["Helpdesk.WebApi/Helpdesk.WebApi.csproj", "Helpdesk.WebApi/"]
COPY ["Helpdesk.Domain/Helpdesk.Domain.csproj", "Helpdesk.Domain/"]
COPY ["Helpdesk.DataAccess/Helpdesk.DataAccess.csproj", "Helpdesk.DataAccess/"]

RUN dotnet restore "Helpdesk.WebApi/Helpdesk.WebApi.csproj"

COPY . .

WORKDIR "/src/Helpdesk.WebApi"
RUN dotnet build "Helpdesk.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Helpdesk.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:5000;
ENV ASPNETCORE_ENVIRONMENT=Production

COPY .assets/support.helpdesk.com/fullchain.pem .
COPY .assets/support.helpdesk.com/privkey.pem .

ENTRYPOINT ["dotnet", "Helpdesk.WebApi.dll"]