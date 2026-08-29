# Mapa de versiones — Módulo Investigación

> La ruta completa del proyecto. Cada versión se especifica **solo cuando
> la anterior está cerrada** (commit + tag). Este mapa da la dirección; el
> spec kit de cada versión da el detalle.
>
> La ruta es la que define
> [modulo_investigacion.md](../../../ProyectosDeAula/docs/modulo_investigacion.md);
> aquí no se inventa nada, se ordena.

## La ruta

| Versión | Qué agrega (acumulativo) | Estado |
|---|---|---|
| **v1** | CRUD completo de las **tablas sin clave foránea**, con los catálogos del Excel cargados | **En curso** ([spec](v1_area_conocimiento/2_spec.md)) |
| v2 | CRUD de las **10 tablas con clave foránea**: las FK como listas desplegables cargadas desde la API, y validación de integridad referencial | Sin especificar |
| v3 | **JWT**, sesiones y control de acceso por roles; CRUD de `usuario`, `rol` y `rol_usuario` solo para administradores | Sin especificar |
| v4 | **10 consultas multitabla** (4+ tablas cada una), dashboard con gráficos, páginas corporativas, responsive/PWA y **publicación** en un servidor | Sin especificar |

## Qué tabla entra en qué versión

Las 19 tablas de la base, repartidas:

| Versión | Tablas |
|---|---|
| **v1** | `area_conocimiento` · `objetivo_desarrollo_sostenible` · `area_aplicacion` · `termino_clave` · `universidad` · `linea_investigacion` |
| v2 | `docente` · `grupo_investigacion` · `semillero` · `participa_grupo` · `participa_semillero` · `grupo_linea` · `semillero_linea` · `ac_linea` · `ods_linea` · `aa_linea` |
| v3 | `rol` · `usuario` · `rol_usuario` |

> **Ojo:** las 19 tablas **existen en la base desde la v1** (Artículo 5 de
> la [constitución](../1_constitution.md)). Lo que reparte esta tabla es
> qué puede **nombrar el código** de cada versión, no qué existe en el
> motor.

## Lo que este ejemplo construye

La v1 de este repositorio se construye sobre **`area_conocimiento`**: una
rebanada vertical completa —controlador, servicio, repositorio,
interfaces, peticiones y prueba sin base de datos— sobre la tabla con más
campos y con 218 filas de catálogo, que es la que da un smoke test de
verdad.

Las demás tablas de la v1 son **ese mismo patrón** con otros nombres. El
equipo que tome este ejemplo lo revisa, y **si está de acuerdo lo retoma y
lo completa; si no, lo rehace a su manera** — lo que no puede es cambiar
la especificación sin pasar por sus compuertas.

## Reglas del mapa

1. **No se anticipa nada de una versión futura** (Artículo 1 de la
   constitución): en la v1 no aparece un `usuario`, ni una FK, ni un token.
2. **Una versión cerrada no se reabre**: los ajustes van en la siguiente.
3. **Regresión obligatoria**: al cerrar la vN, los criterios de todas las
   versiones anteriores deben seguir pasando.
4. El repositorio siempre muestra la **versión en curso, funcionando**.
