#!/bin/bash
# ==============================================================
# Inicializador de SQL Server (contenedor sqlserver-init).
#
# ¿Por qué existe este script? SQL Server NO ejecuta los scripts que se
# le monten: alguien tiene que conectarse al motor y correrlos. Ese
# alguien es este contenedor, que hace su trabajo UNA vez y termina.
#
# Crea la base investigacion_local y ejecuta investigacion.sql SOLO si la base no existe
# todavía. Es idempotente: correrlo mil veces no daña nada.
#
# La contraseña NO está escrita aquí: llega por la variable de entorno
# MSSQL_SA_PASSWORD que le pasa el docker-compose.yml.
# ==============================================================

# si cualquier comando falla, detenerse aquí en vez de seguir a ciegas
set -e

SQLCMD=/opt/mssql-tools18/bin/sqlcmd   # el cliente de línea de comandos
SERVER=sqlserver                       # el nombre del servicio en el compose
DB=investigacion_local

echo "[init] ¿Existe ya la base $DB?"
EXISTE=$($SQLCMD -S $SERVER -U sa -P "$MSSQL_SA_PASSWORD" -C -h -1 -W \
  -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM sys.databases WHERE name = '$DB'")

if [ "$EXISTE" = "1" ]; then
    echo "[init] Ya existe. No se hace nada."
    exit 0
fi

echo "[init] Creando la base $DB..."
$SQLCMD -S $SERVER -U sa -P "$MSSQL_SA_PASSWORD" -C -Q "CREATE DATABASE $DB"

echo "[init] Ejecutando investigacion.sql (19 tablas y los catálogos)..."
$SQLCMD -S $SERVER -U sa -P "$MSSQL_SA_PASSWORD" -C -d $DB -i /scripts/investigacion.sql

echo "[init] Listo: la base quedó creada y sembrada."
