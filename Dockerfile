# ===== Estágio 1: Build =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia o csproj e restaura dependências (cache de layers)
COPY ["EmprestimoLivros.csproj", "./"]
RUN dotnet restore "EmprestimoLivros.csproj"

# Copia todo o resto e builda
COPY . .
RUN dotnet publish "EmprestimoLivros.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===== Estágio 2: Runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Render injeta a porta na variável PORT
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "EmprestimoLivros.dll"]