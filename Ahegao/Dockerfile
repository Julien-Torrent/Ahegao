FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN ["apt-get", "update"]
RUN ["apt-get", "-y", "install", "libgdiplus"]
RUN ["apt-get", "-y", "install", "xvfb", "libfontconfig", "wkhtmltopdf"]
RUN ["apt-get", "-y", "install", "libc6-dev"]
RUN ["apt-get", "-y", "install", "openssl"]
RUN ["apt-get", "-y", "install", "libssl1.0"]

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
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