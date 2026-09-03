# Contratos HTTP — Versión 1: los 7 endpoints exactos

> Base: `http://localhost:8070` · Documentación interactiva en
> `/swagger`. Lo que este documento dice se cumple **al pie de la letra**
> (Artículo 9): un cliente puede exigirlo sin leer el código.

## 0. Convenciones globales

**Sobre de lectura** (listados):

```json
{ "tabla": "area_conocimiento", "limite": 1000, "total": 218, "datos": [ … ] }
```

**Sobre de error**:

```json
{ "estado": 422, "mensaje": "Datos inválidos.", "detalle": "…",
  "errores": ["El campo granArea es obligatorio."] }
```

`errores[]` aparece **solo** en el 422.

**Los nombres de los campos JSON van en camelCase** (`granArea`,
`filasAfectadas`), que es lo que ASP.NET Core hace **por defecto**: no hay
que configurar nada, y por lo tanto no hay nada que se pueda configurar mal.

> **Ojo con la diferencia entre la ruta y el cuerpo.** La ruta es
> `/api/area_conocimiento` —con guion bajo, porque nombra la tabla
> (Artículo 10)— y el cuerpo usa `granArea`. No es una inconsistencia: la
> ruta identifica **el recurso**, y el JSON sigue la convención de quien lo
> consume. El front de la v4 va a leer `granArea` sin traducir nada.
>
> Y algo que se ve mejor así: **el JSON no es una ventana a la tabla.** Si
> mañana la columna se renombra, el contrato no tiene por qué cambiar —
> justamente porque no son lo mismo.

**Catálogo de códigos** (Artículo 10):

| Situación | Código |
|---|---|
| Lectura correcta · escritura correcta | **200** |
| Lectura sin filas activas | **204** (sin cuerpo) |
| Regla de negocio rota (`limite` ≤ 0, `PATCH` sin campos) | **400** |
| Cuerpo inválido: falta un campo, tipo equivocado, texto muy largo | **422** |
| El código no existe, o está inactivo | **404** |
| La base rechaza (llave duplicada) o falla | **500** (motor en `detalle`) |

## 1. `GET /` — Diagnóstico

```
GET /
→ 200 { "mensaje": "API Investigación — módulo de áreas de conocimiento",
        "version": "v1",
        "contratos": "/swagger" }
```

**Sin desenlaces de error, y a propósito:** no recibe parámetros ni cuerpo,
y no consulta la base. Si este endpoint no responde 200, el problema no es
de contrato — es que la API no está arriba.

## 2. `GET /api/area_conocimiento[?limite=N]` — Listar

```
GET /api/area_conocimiento
→ 200 { "tabla":"area_conocimiento", "limite":1000, "total":218,
        "datos":[ {"id":"1A01","granArea":"Ciencias Naturales",
                   "area":"Matemáticas","disciplina":"Matemáticas puras"}, … ] }

GET /api/area_conocimiento?limite=3
→ 200 { …, "limite":3, "total":3, "datos":[ 3 elementos ] }

→ 204 (sin cuerpo) si no hay filas activas
→ 400 si limite <= 0
```

Devuelve **solo** las filas con `activo = 1`. El campo `activo` **no viaja
en la respuesta**: es un detalle interno, no parte del catálogo.

## 3. `GET /api/area_conocimiento/{id}` — Obtener una

```
GET /api/area_conocimiento/1A01
→ 200 { "id":"1A01", "granArea":"Ciencias Naturales",
        "area":"Matemáticas", "disciplina":"Matemáticas puras" }

GET /api/area_conocimiento/9Z99          ← no existe
→ 404 { "estado":404, "mensaje":"Área de conocimiento no encontrada.",
        "detalle":"No existe un área con el código 9Z99." }
```

Una fila **inactiva** responde igual: 404 (C4).

## 4. `POST /api/area_conocimiento` — Crear

Cuerpo (petición `AreaConocimientoCrear` — los cuatro obligatorios):

```
POST /api/area_conocimiento
body {"id":"9Z01","granArea":"Ciencias Naturales",
      "area":"Matemáticas","disciplina":"Teoría de números"}
→ 200 { "estado":200, "mensaje":"Área de conocimiento creada exitosamente." }

body {"id":"9Z01","area":"Matemáticas"}        ← falta granArea y disciplina
→ 422 { "estado":422, "mensaje":"Datos inválidos.",
        "errores":["El campo granArea es obligatorio.",
                   "El campo disciplina es obligatorio."] }

body {"id":"DEMASIADOLARGO", …}                ← el id excede 6 caracteres
→ 422

body {"id":"1A01", …}                          ← código duplicado (PK)
→ 500 con el error del motor en detalle
```

El registro nace con `activo = 1`. **El cuerpo no acepta `activo`**: si
llega, se ignora (§4 del modelo de datos).

## 5. `PUT /api/area_conocimiento/{id}` — Reemplazo COMPLETO

```
PUT /api/area_conocimiento/9Z01
body {"granArea":"Humanidades","area":"Filosofía","disciplina":"Ética"}
→ 200 { "estado":200, "mensaje":"Área de conocimiento reemplazada.",
        "filasAfectadas":1 }

body {"granArea":"Humanidades","disciplina":"Ética"}   ← falta area
→ 422 { …, "errores":["El campo area es obligatorio."] }

PUT /api/area_conocimiento/9Z99                          ← no existe
→ 404
```

**Los tres campos son obligatorios**: reemplazar es poner todo de nuevo. El
`id` no va en el cuerpo — identifica la fila, no se cambia.

## 6. `PATCH /api/area_conocimiento/{id}` — Actualización PARCIAL

```
PATCH /api/area_conocimiento/9Z01
body {"disciplina":"Ética aplicada"}          ← solo lo que cambia
→ 200 { "estado":200, "mensaje":"Área de conocimiento actualizada.",
        "filasAfectadas":1 }

body {"granArea":"Humanidades","disciplina":"Ética"}   ← el MISMO cuerpo que
                                                          el PUT rechazó
→ 200                                                   ← aquí es válido

body {}                                       ← nada que actualizar
→ 400 { "estado":400, "mensaje":"Parámetros inválidos.",
        "detalle":"No se envió ningún campo para actualizar." }

PATCH /api/area_conocimiento/9Z99
→ 404
```

**Esta pareja es la lección del contrato:** el mismo cuerpo da 422 en `PUT`
y 200 en `PATCH`. No es un capricho — reemplazar exige todo, actualizar
solo lo enviado.

## 7. `DELETE /api/area_conocimiento/{id}` — Eliminar (LÓGICO)

```
DELETE /api/area_conocimiento/9Z01
→ 200 { "estado":200, "mensaje":"Área de conocimiento eliminada.",
        "filasAfectadas":1 }

DELETE /api/area_conocimiento/9Z01           ← segunda vez: ya está inactiva
→ 404

DELETE /api/area_conocimiento/9Z99           ← nunca existió
→ 404
```

**La fila no se borra:** queda con `activo = 0` y desaparece de los
listados. Comprobarlo es el criterio 5 de la spec: el `total` vuelve a 218
y la fila sigue en la base.


---

## 8. El contrato de la PANTALLA

Los siete apartados anteriores son el contrato de la API con **cualquiera** que
la consuma. Este es el de la pantalla con **quien la usa**, y son dos contratos
distintos: el front es *un* cliente de la API, no *el* cliente.

| Pantalla | Dirección | Qué ofrece |
|---|---|---|
| Inicio | <http://localhost:8071/> | La entrada, con el enlace al catálogo |
| Áreas de conocimiento | <http://localhost:8071/areas-de-conocimiento> | La tabla, «Agregar», «Editar» y «Retirar» |

**Cada pantalla tiene dirección propia**, no una con el nombre de la tabla como
parámetro (Artículo 10.1 · sección 6.1 de la metodología). Se puede guardar
como marcador, poner en el menú y mandar por correo.

### Qué pantalla llama a qué endpoint

| Lo que hace el usuario | Lo que manda el front |
|---|---|
| Abrir la pantalla | `GET /api/area_conocimiento?limite=1000` |
| «Agregar» y guardar | `POST /api/area_conocimiento` |
| «Editar» | `GET /api/area_conocimiento/{id}` (ya viene en el listado) |
| «Guardar la ficha completa» | `PUT /api/area_conocimiento/{id}` |
| «Guardar solo lo que cambié» | `PATCH /api/area_conocimiento/{id}` con **solo** lo diligenciado |
| «Retirar», tras confirmar | `DELETE /api/area_conocimiento/{id}` |

### Cómo traduce el front los errores de la API

El front **no repite** ninguna validación de la API: manda, y muestra lo que
vuelva. Su servicio traduce el sobre a una lista de textos:

| Lo que responde la API | Lo que ve el usuario |
|---|---|
| **422** con `errores[]` | Un aviso rojo por cada error, con el texto que mandó la API |
| **400 / 404 / 500** con `{mensaje, detalle}` | Un aviso rojo con esos dos textos |
| **La API no responde** | «El servicio no está disponible. ¿Está arriba la API?» |

> **La última fila es la que demuestra la arquitectura.** Con la API apagada la
> pantalla **sigue en pie** —cabecera, menú, pie— y muestra ese aviso **sin un
> solo dato**. Si el front pudiera llegar a SQL Server por su cuenta, seguiría
> mostrando el catálogo.
>
> Lo comprueba `pruebas_humo/humo_front.py`, que apaga la API a propósito.
