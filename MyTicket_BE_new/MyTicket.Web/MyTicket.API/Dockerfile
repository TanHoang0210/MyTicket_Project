#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyTicket.API/MYTICKET.WEB.API.csproj", "MyTicket.API/"]
COPY ["MYTICKET.BASE.API/MYTICKET.BASE.API.csproj", "MYTICKET.BASE.API/"]
COPY ["Myticket.Service/MYTICKET.WEB.SERVICE.csproj", "Myticket.Service/"]
COPY ["MYTICKET.BASE.SERVICE/MYTICKET.BASE.SERVICE.csproj", "MYTICKET.BASE.SERVICE/"]
COPY ["MYTICKET.UTILS/MYTICKET.UTILS.csproj", "MYTICKET.UTILS/"]
COPY ["MYTICKET.WEB.Infrastructure/MYTICKET.WEB.Infrastructure.csproj", "MYTICKET.WEB.Infrastructure/"]
COPY ["MYTICKET.BASE.DOMAIN/MYTICKET.BASE.Infrastructure.csproj", "MYTICKET.BASE.DOMAIN/"]
COPY ["ENTITIES/MYTICKET.BASE.ENTITIES.csproj", "ENTITIES/"]
COPY ["MYTICKET.WEB.DOMAIN/MYTICKET.WEB.DOMAIN.csproj", "MYTICKET.WEB.DOMAIN/"]
COPY ["MYTICKET.BASE.IdentityServer/MYTICKET.BASE.IdentityServer.csproj", "MYTICKET.BASE.IdentityServer/"]
RUN dotnet restore "MyTicket.API/MYTICKET.WEB.API.csproj"
COPY . .
WORKDIR "/src/MyTicket.API"
RUN dotnet build "MYTICKET.WEB.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MYTICKET.WEB.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MYTICKET.WEB.API.dll"]