FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY src/PersonalWebsite.Api/PersonalWebsite.Api.csproj src/PersonalWebsite.Api/
COPY src/PersonalWebsite.Infrastructure/PersonalWebsite.Infrastructure.csproj src/PersonalWebsite.Infrastructure/
COPY src/PersonalWebsite.Core/PersonalWebsite.Core.csproj src/PersonalWebsite.Core/
COPY test/PersonalWebsite.Test/PersonalWebsite.Test.csproj test/PersonalWebsite.Test/
COPY PersonalWebsite.sln .

RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "PersonalWebsite.Api.dll"]
