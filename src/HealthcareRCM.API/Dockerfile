FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY HealthcareRCM.API/HealthcareRCM.API.csproj HealthcareRCM.API/
RUN dotnet restore HealthcareRCM.API/HealthcareRCM.API.csproj
COPY HealthcareRCM.API/ HealthcareRCM.API/
RUN dotnet publish HealthcareRCM.API/HealthcareRCM.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "HealthcareRCM.API.dll"]
