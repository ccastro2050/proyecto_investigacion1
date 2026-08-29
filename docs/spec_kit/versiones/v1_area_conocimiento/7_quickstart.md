# Quickstart — Versión 1: arranque y smoke test

## 1. Arranque

Un solo comando, desde la raíz del proyecto:

```powershell
docker compose up -d --build
```

La primera vez tarda unos minutos: descarga la imagen de SQL Server,
espera a que el motor responda, crea la base con sus 19 tablas y sus 218
áreas, y compila la API. Al terminar:

| Qué | Dónde |
|---|---|
| API — diagnóstico | http://localhost:8070/ |
| Documentación interactiva | http://localhost:8070/swagger |
| Listado de áreas | http://localhost:8070/api/area_conocimiento |
| SQL Server (SSMS o SQLTools, opcional) | `localhost,11470` · usuario `sa` |

> **¿La contraseña?** Está en el `docker-compose.yml`, a la vista: esta es
> una plantilla didáctica y esa es la excepción declarada en el Artículo 7
> de la [constitución](../../1_constitution.md). **Para correr el sistema
> no hace falta** —el compose se la entrega a los contenedores—; solo se
> necesita para conectarse por fuera con SSMS o SQLTools.
>
> **En su proyecto de aula eso no se copia:** ahí va en un `.env` fuera de
> git, con un `.env.example` adentro.

**Si cambia la contraseña**, no basta con editar el compose:

```powershell
docker compose down -v        # -v borra el volumen: la base olvida el sa viejo
docker compose up -d --build
```

Sin el `-v`, el usuario `sa` sigue existiendo dentro del volumen con la
clave anterior y el login falla — con un error que no menciona los
volúmenes por ninguna parte.

## 2. Smoke test

Los comandos van **numerados igual que los criterios de aceptación** de
[2_spec.md](2_spec.md). Si los siete pasan, la versión está terminada.

```powershell
# 1. Un solo comando: la API responde y dice qué versión es
curl http://localhost:8070/
#    → {"mensaje":"...","version":"v1","contratos":"/swagger"}

# 2. Listar: 218 áreas; con límite, exactamente 3
curl http://localhost:8070/api/area_conocimiento
#    → {"tabla":"area_conocimiento","limite":1000,"total":218,"datos":[...]}
curl "http://localhost:8070/api/area_conocimiento?limite=3"
#    → total: 3

# 3. Obtener una que existe, y una que no
curl http://localhost:8070/api/area_conocimiento/1A01
#    → {"id":"1A01","granArea":"Ciencias Naturales","area":"Matemáticas",
#       "disciplina":"Matemáticas puras"}
curl -i http://localhost:8070/api/area_conocimiento/9Z99
#    → 404 con {"estado":404,"mensaje":"Área de conocimiento no encontrada.",...}

# 4. El ciclo de los cinco verbos
curl -X POST http://localhost:8070/api/area_conocimiento `
  -H "Content-Type: application/json" `
  -d '{"id":"9Z01","granArea":"Ciencias Naturales","area":"Matemáticas","disciplina":"Teoría de números"}'
#    → 200 creada

curl -X PUT http://localhost:8070/api/area_conocimiento/9Z01 `
  -H "Content-Type: application/json" `
  -d '{"granArea":"Humanidades","area":"Filosofía","disciplina":"Ética"}'
#    → 200 filasAfectadas: 1

curl -X PATCH http://localhost:8070/api/area_conocimiento/9Z01 `
  -H "Content-Type: application/json" -d '{"disciplina":"Ética aplicada"}'
#    → 200 filasAfectadas: 1

curl http://localhost:8070/api/area_conocimiento/9Z01
#    → la fila con Humanidades / Filosofía / Ética aplicada

# 4b. La pareja que enseña la diferencia: MISMO cuerpo, dos verbos
curl -i -X PUT http://localhost:8070/api/area_conocimiento/9Z01 `
  -H "Content-Type: application/json" `
  -d '{"granArea":"Humanidades","disciplina":"Ética"}'
#    → 422: al PUT le falta 'area' y reemplazar exige todo

curl -i -X PATCH http://localhost:8070/api/area_conocimiento/9Z01 `
  -H "Content-Type: application/json" `
  -d '{"granArea":"Humanidades","disciplina":"Ética"}'
#    → 200: al PATCH le basta con lo enviado

# 5. El borrado es LÓGICO, y se comprueba
curl http://localhost:8070/api/area_conocimiento          # → total: 219
curl -X DELETE http://localhost:8070/api/area_conocimiento/9Z01
#    → 200 filasAfectadas: 1
curl http://localhost:8070/api/area_conocimiento          # → total: 218 otra vez
curl -i -X DELETE http://localhost:8070/api/area_conocimiento/9Z01
#    → 404: para la API ya no existe

#    …pero la fila SIGUE en la base. Comprobarlo:
docker compose exec sqlserver bash -c '/opt/mssql-tools18/bin/sqlcmd `
  -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -d investigacion_local `
  -Q "SELECT id, activo FROM area_conocimiento WHERE id = ''9Z01''"'
#    → 9Z01 | 0

# 6. La validación es la frontera: nada de esto llega a la base
curl -i -X POST http://localhost:8070/api/area_conocimiento `
  -H "Content-Type: application/json" `
  -d '{"id":"9Z02","area":"Matemáticas","disciplina":"X"}'
#    → 422 con errores: falta granArea

curl -i -X POST http://localhost:8070/api/area_conocimiento `
  -H "Content-Type: application/json" `
  -d '{"id":"1A01","granArea":"X","area":"Y","disciplina":"Z"}'
#    → 500: la llave primaria ya existe (la defiende la base)

# 7. La prueba de capas: sin base de datos
docker compose exec api-investigacion dotnet run --project pruebas
#    → todas las verificaciones pasan, con un repositorio FALSO en memoria
```

## 3. Regresión

Esta es la primera versión: no hay nada anterior que probar. **Desde la
v2**, esta sección conserva los smokes de todas las versiones cerradas y
todos deben seguir pasando antes de cerrar la nueva.

## 4. Si algo falla

| Síntoma | Causa probable |
|---|---|
| `Login failed for user 'sa'` | Se cambió la contraseña sin `docker compose down -v` (§1) |
| La API responde 500 en todo, con "No address associated with hostname" | La API arrancó antes que la base. El compose lo evita con `depends_on`; si pasa, `docker compose restart api-investigacion` |
| `total: 0` o menos de 218 | El inicializador no corrió o falló. `docker compose logs sqlserver-init` |
| El contenedor de SQL Server se reinicia solo | Contraseña que no cumple la política (8+ caracteres, mayúscula, minúscula, dígito y símbolo) o poca memoria: pide ~2 GB |
| Un inactivo aparece en el listado | A alguna consulta le falta `WHERE activo = 1` ([3_plan](3_plan.md) §4.2) |
| `bad interpreter: /bin/bash^M` | `db/init.sh` se guardó con finales de línea de Windows. Es lo que previene `*.sh text eol=lf` en `.gitattributes` |
