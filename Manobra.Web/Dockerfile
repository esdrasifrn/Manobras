#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Manobra.Web/Manobra.Web.csproj", "Manobra.Web/"]
RUN dotnet restore "Manobra.Web/Manobra.Web.csproj"
COPY . .
WORKDIR "/src/Manobra.Web"
RUN dotnet build "Manobra.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Manobra.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Manobra.Web.dll"]