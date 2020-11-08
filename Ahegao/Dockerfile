#FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim-arm64v8 AS base
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim-arm64v8 AS build
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Ahegao/Ahegao.csproj", "Ahegao/"]
RUN dotnet restore "Ahegao/Ahegao.csproj"
COPY . .
WORKDIR "/src/Ahegao"
RUN dotnet build "Ahegao.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ahegao.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ahegao.dll"]