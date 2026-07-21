# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY . .
RUN dotnet restore Outlander.Demo/Outlander.Demo.csproj

# Publish
RUN dotnet publish Outlander.Demo/Outlander.Demo.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render uses PORT env var; bind Kestrel to it
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
EXPOSE 10000

ENTRYPOINT ["dotnet", "Outlander.Demo.dll"]
