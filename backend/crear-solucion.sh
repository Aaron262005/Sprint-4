#!/bin/bash
# Ejecuta esto UNA sola vez, parado en la carpeta Sprint4-Backend, con el SDK de .NET 8 instalado.
# Genera Sprint4.Backend.sln y la enlaza con los 4 proyectos ya creados.
# En Visual Studio también puedes simplemente hacer doble clic al .sln resultante.

set -e

dotnet new sln -n Sprint4.Backend

dotnet sln add src/Sprint4.Backend.Domain/Sprint4.Backend.Domain.csproj
dotnet sln add src/Sprint4.Backend.Application/Sprint4.Backend.Application.csproj
dotnet sln add src/Sprint4.Backend.Infrastructure/Sprint4.Backend.Infrastructure.csproj
dotnet sln add src/Sprint4.Backend.API/Sprint4.Backend.API.csproj

dotnet restore

echo "Listo. Abre Sprint4.Backend.sln en Visual Studio y corre el proyecto Sprint4.Backend.API."
