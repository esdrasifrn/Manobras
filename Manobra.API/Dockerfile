#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Manobra.API/Manobra.API.csproj", "Manobra.API/"]
COPY ["Manobra.Application/Manobra.Application.csproj", "Manobra.Application/"]
COPY ["Manobra.Domain/Manobra.Domain.csproj", "Manobra.Domain/"]
COPY ["Manobra.Infra/Manobra.Infra.csproj", "Manobra.Infra/"]
COPY ["SDK.DependencyInjection/SDK.DependencyInjection.csproj", "SDK.DependencyInjection/"]
RUN dotnet restore "Manobra.API/Manobra.API.csproj"
COPY . .
WORKDIR "/src/Manobra.API"
RUN dotnet build "Manobra.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Manobra.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Manobra.API.dll"]