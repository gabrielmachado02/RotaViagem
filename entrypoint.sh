#!/bin/bash
set -e

echo "Aguardando SQL Server ficar disponível..."

# Aguarda o SQL Server ficar disponível
until nc -z sqlserver 1433; do
  echo "SQL Server (sqlserver:1433) ainda não está disponível - aguardando..."
  sleep 2
done

echo "SQL Server está disponível!"
echo "Executando migrations..."

# Executa as migrations
cd /app/src/RotaViagem.API
dotnet ef database update --no-build --project ../RotaViagem.Infrastructure --startup-project .

echo "Migrations executadas com sucesso!"
echo "Iniciando a aplicação..."

# Inicia a aplicação
cd /app
exec dotnet RotaViagem.API.dll 