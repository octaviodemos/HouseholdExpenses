# =============================================================================
# Estágio 1: build — SDK completo para restaurar, compilar e publicar a API
# Contexto Docker: pasta backend/ (um nível acima deste Dockerfile)
# =============================================================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar apenas os ficheiros .csproj primeiro: quando o código muda mas as
# dependências não, o Docker reutiliza a camada do restore (melhor cache).
COPY ["HouseholdExpenses.Api/HouseholdExpenses.Api.csproj", "HouseholdExpenses.Api/"]
COPY ["HouseholdExpenses.Application/HouseholdExpenses.Application.csproj", "HouseholdExpenses.Application/"]
COPY ["HouseholdExpenses.Domain/HouseholdExpenses.Domain.csproj", "HouseholdExpenses.Domain/"]
COPY ["HouseholdExpenses.Infrastructure/HouseholdExpenses.Infrastructure.csproj", "HouseholdExpenses.Infrastructure/"]
COPY ["HouseholdExpenses.Communication/HouseholdExpenses.Communication.csproj", "HouseholdExpenses.Communication/"]
COPY ["HouseholdExpenses.Exception/HouseholdExpenses.Exception.csproj", "HouseholdExpenses.Exception/"]
# Referenciado em HouseholdExpenses.sln — necessário para restaurar a solution antes de existir o código completo
COPY ["HouseholdExpenses.Tests/HouseholdExpenses.Tests.csproj", "HouseholdExpenses.Tests/"]

# Ficheiro da solution na raiz do contexto (backend/)
COPY ["HouseholdExpenses.sln", "."]

# Restaurar pacotes NuGet para todos os projetos da solution
RUN dotnet restore "HouseholdExpenses.sln"

# Copiar o restante código-fonte e compilar em Release
COPY . .
RUN dotnet build "HouseholdExpenses.sln" -c Release --no-restore

# Publicar só o projeto da API para uma pasta estável (artefatos finais)
RUN dotnet publish "HouseholdExpenses.Api/HouseholdExpenses.Api.csproj" -c Release -o /app/publish --no-restore --no-build

# =============================================================================
# Estágio 2: runtime — imagem aspnet (menor), só com o necessário para executar
# =============================================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Artefactos publicados no estágio anterior
COPY --from=build /app/publish .

# Porta em que a aplicação escuta dentro do contentor
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Arranque da API (assembly principal gerado pelo publish)
ENTRYPOINT ["dotnet", "HouseholdExpenses.Api.dll"]
