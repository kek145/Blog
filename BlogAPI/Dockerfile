FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /src

COPY BlogAPI.DAL/*csproj BlogAPI.DAL/
COPY BlogAPI.BL/*csproj BlogAPI.BL/
COPY BlogAPI.Domain/*csproj BlogAPI.Domain/
COPY BlogAPI.Security/*csproj BlogAPI.Security/
COPY BlogAPI.UI/*csproj BlogAPI.UI/

RUN dotnet restore BlogAPI.DAL/
RUN dotnet restore BlogAPI.BL/
RUN dotnet restore BlogAPI.Domain/
RUN dotnet restore BlogAPI.Security/
RUN dotnet restore BlogAPI.UI/

COPY BlogAPI.DAL/. BlogAPI.DAL/
COPY BlogAPI.BL/. BlogAPI.BL/
COPY BlogAPI.Domain/. BlogAPI.Domain/
COPY BlogAPI.Security/. BlogAPI.Security/

FROM build-env AS build-app
WORKDIR /src/BlogAPI.UI

COPY BlogAPI.UI/. .

RUN dotnet build -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build-app /app/publish .

EXPOSE 80

ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "BlogAPI.UI.dll"]