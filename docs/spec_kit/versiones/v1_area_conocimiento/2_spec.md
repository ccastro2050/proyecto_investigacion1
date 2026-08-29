# Especificación — Versión 1: `area_conocimiento` + SQL Server

> **Versión 1** ([mapa](../0_mapa_versiones.md)) · La primera rebanada
> vertical del módulo Investigación: una tabla, sus siete endpoints y las
> tres capas completas. Ante conflicto con este documento, manda la
> [constitución](../../1_constitution.md).

## 1. Propósito de la v1

Construir la API del catálogo de **áreas de conocimiento** de punta a
punta: controlador, servicio, repositorio e interfaces, contra SQL Server
y en un solo comando.

La v1 no busca cubrir el módulo: busca **dejar el patrón montado y
verificado**. Las demás tablas sin clave foránea son este mismo patrón con
otros nombres.

## 2. Alcance

**Incluye**

- El CRUD completo de `area_conocimiento`: listar (con límite), obtener
  por código, crear, reemplazar, actualizar parcialmente y eliminar.
- **Borrado lógico**: `DELETE` marca `activo = 0` y los listados filtran
  los inactivos (Artículo 6).
- Un endpoint de diagnóstico y la documentación interactiva en `/swagger`.
- La prueba de capas: el servicio corriendo con un repositorio falso, sin
  base de datos.

**NO incluye** — y no se anticipa nada de esto (Artículo 1)

- Ninguna otra tabla de las 19, aunque existan todas en la base.
- Claves foráneas, listas desplegables ni integridad referencial: eso es
  la v2.
- Autenticación, JWT, roles ni usuarios: eso es la v3.
- Frontend, dashboard ni consultas multitabla: eso es la v4.
- **Reactivar** un registro inactivo (`activo = 1`). Nadie lo pidió; si
  hace falta, se especifica en una versión posterior.
- Búsqueda por texto, ordenamiento ni paginación con desplazamiento: el
  único filtro de la v1 es `?limite`.

## 3. Requisitos funcionales

### RF1 — Listar áreas de conocimiento (GET + query string)
`GET /api/area_conocimiento` → 200 con el sobre
`{tabla, limite, total, datos:[…]}`.
- Devuelve **solo las activas**.
- Parámetro opcional `limite` (entero > 0; por defecto 1000).
- Sin filas activas → **204** sin cuerpo.

### RF2 — Obtener por código (GET + parámetro de ruta)
`GET /api/area_conocimiento/{id}` → 200 con el área.
- El `id` es **texto** (`1A01`), no un número.
- Inexistente **o inactiva** → 404.

### RF3 — Crear (POST + cuerpo completo)
`POST /api/area_conocimiento` con `{id, gran_area, area, disciplina}`.
- Los cuatro campos son obligatorios.
- Nace con `activo = 1`.
- Código ya existente → **500** (ver C8: por qué 500 y no 409).

### RF4 — Reemplazar (PUT + cuerpo completo)
`PUT /api/area_conocimiento/{id}` con `{gran_area, area, disciplina}`.
- **Los tres campos son obligatorios**: es un reemplazo. Falta uno → 422.
- Devuelve `filasAfectadas`; inexistente → 404.

### RF5 — Actualizar parcialmente (PATCH + cuerpo parcial)
`PATCH /api/area_conocimiento/{id}` con los campos que se quieran cambiar.
- Solo se modifican los enviados.
- Cuerpo vacío → 400 (no hay nada que actualizar).
- Devuelve `filasAfectadas`; inexistente → 404.

### RF6 — Eliminar (DELETE, borrado lógico)
`DELETE /api/area_conocimiento/{id}` marca `activo = 0`.
- Devuelve `filasAfectadas`.
- Inexistente **o ya inactiva** → 404.
- El registro **deja de existir para la API**, pero sigue siendo
  recuperable: eso es lo que significa que el borrado sea lógico.

### RF7 — Diagnóstico
`GET /` → JSON con mensaje, versión (`"v1"`) y la ruta de los contratos.

## 4. Requisitos no funcionales

- **Un solo comando**: `docker compose up -d --build` (Artículo 4).
- **Tres capas con interfaces** y solo el ensamblador conociendo clases
  concretas (Artículo 3).
- **SQL a mano y siempre parametrizado** (`@parametro`), sin ORM de
  entidades (Artículo 2).
- **Todo en español** (Artículo 8).
- El listado completo (218 filas) responde en **menos de 1 segundo** en un
  equipo de escritorio corriente.
- La API publica su documentación interactiva en `/swagger`.

## 5. Criterios de aceptación

1. **Un solo comando.** `docker compose up -d --build` deja corriendo SQL
   Server —con la base creada, sus 19 tablas y **218 filas** en
   `area_conocimiento`— y la API. `GET http://localhost:8070/` responde el
   diagnóstico con `"version":"v1"`.
2. **Listar.** `GET /api/area_conocimiento` devuelve
   `{tabla:"area_conocimiento", total:218, …}` y
   `GET /api/area_conocimiento?limite=3` devuelve **exactamente 3**.
3. **Obtener.** `GET /api/area_conocimiento/1A01` devuelve
   `{gran_area:"Ciencias Naturales", area:"Matemáticas",
   disciplina:"Matemáticas puras"}`; `GET /api/area_conocimiento/9Z99`
   responde **404** con mensaje claro.
4. **Ciclo de los cinco verbos.** `POST` crea `9Z01` → `PUT` lo reemplaza
   completo → `PATCH` le cambia solo `disciplina` → `GET` lo confirma →
   `DELETE` lo desactiva, y un **segundo** `DELETE` responde **404**.
   Además, un `PUT` sin el campo `area` responde **422** mientras el
   **mismo cuerpo** enviado por `PATCH` responde **200** — la diferencia
   entre reemplazar y actualizar.
5. **El borrado es lógico, y se verifica.** Después de crear `9Z01` el
   listado dice `total: 219`; después del `DELETE` vuelve a decir
   `total: 218`, **y la fila sigue en la base** con `activo = 0`
   (comprobable con una consulta directa).
6. **La validación es la frontera.** `POST` sin `gran_area` → **422** con
   `errores:[…]`; `POST` con un `id` de más de 6 caracteres → **422**;
   `POST` con un código que ya existe → **500** con el error del motor en
   `detalle`. En ninguno de los tres casos se toca la base.
7. **Prueba de capas.** El proyecto `pruebas/` ejecuta el servicio con un
   **repositorio de mentiras**: otra implementación de la misma interfaz
   que guarda las filas en una lista en memoria, en vez de hablar con la
   base. Todas sus verificaciones pasan **con SQL Server apagado** — que es
   la prueba de que las capas están desacopladas ([3_plan](3_plan.md) §4.6).

## 6. Clarificaciones

> **Qué es esta sección:** el registro de las ambigüedades detectadas
> ANTES de planear, con la respuesta acordada y su razón. Es la
> **compuerta 1** del método: mientras quede un `[NECESITA ACLARACIÓN: …]`
> en los requisitos de arriba, esta versión no pasa a la planeación.

| # | La pregunta | La respuesta, con su razón | Dónde quedó |
|---|---|---|---|
| C1 | El script declara `area_conocimiento.id` como `INT`, pero los datos del Excel son códigos como `1A01`. ¿Cuál manda? | **Mandan los datos: `VARCHAR(6)`.** Un entero no puede guardar `1A01`, así que el script no podría cargar su propio catálogo. Arrastra a `ac_linea.area_conocimiento`, que lo referencia | `5_data_model` §2 · `db/investigacion.sql` |
| C2 | `disciplina` está declarada `VARCHAR(60)` y el valor más largo del Excel tiene **124** caracteres. ¿Se recorta el dato o se agranda la columna? | **Se agranda a `VARCHAR(150)`.** Recortar un catálogo oficial lo falsea; el margen extra no cuesta nada | `5_data_model` §2 |
| C3 | Ninguna tabla del módulo trae columna `activo`, pero la metodología exige borrado lógico. ¿Se agrega o se borra físicamente? | **Se agrega `activo BIT NOT NULL DEFAULT 1`** a las 16 tablas del módulo. Sin ella, la versión contradice su propia rúbrica | Artículo 6 · RF6 |
| C4 | Un registro inactivo, ¿se puede consultar por su código? | **No: responde 404.** Si el listado los filtra, individualmente tampoco existen. Ser coherente importa más que ser permisivo | RF2 · RF6 |
| C5 | ¿Y un segundo `DELETE` sobre el mismo código? | **404**, por consecuencia directa de C4: para la API ya no existe | RF6 · criterio 4 |
| C6 | ¿Se puede reactivar un registro (`activo = 1`)? | **No en la v1.** Es una operación de negocio que nadie pidió. Queda en el NO incluye; si hace falta, se especifica | §2 Alcance |
| C7 | `?limite=0` o negativo, ¿es 422 o 400? | **400.** La forma del dato es correcta —sí es un entero—; lo que se rompe es una regla de negocio. El 422 se reserva para el cuerpo mal formado | RF1 · Artículo 10 |
| C8 | Crear con un código que ya existe, ¿409 o 500? | **500**, con el error del motor en `detalle`. En la v1 la llave la defiende la base, no la API; convertirlo en 409 sería lógica de negocio que esta versión no pide | RF3 · criterio 6 |
| C9 | El catálogo trae **"Cienias Naturales"** (sin la `c`) en 36 de las 218 filas. ¿Se corrige o se carga tal cual? | **Se corrige a "Ciencias Naturales"** al generar las semillas, y queda anotado en la cabecera del script. Es un error de digitación de la fuente, no un dato; cargarlo tal cual lo perpetúa en pantalla y en los informes | `db/investigacion.sql` · `5_data_model` §3 |

## 7. Definición de TERMINADA

La v1 está terminada —y solo entonces se escribe la spec de la v2— cuando:

1. Los **7 criterios de aceptación** pasan, verificados con el smoke test
   de [7_quickstart.md](7_quickstart.md) **corrido por una persona**.
2. La lista de [9_checklist.md](9_checklist.md) está en verde y firmada.
3. No queda ningún `[NECESITA ACLARACIÓN: …]` en este documento.
4. Se hace commit y **tag `v1`** (Artículo 1).
