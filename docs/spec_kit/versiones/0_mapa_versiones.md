# Mapa de versiones — Módulo Investigación

> La ruta completa del proyecto. Cada versión se especifica **solo cuando
> la anterior está cerrada** (commit + tag). Este mapa da la dirección; el
> spec kit de cada versión da el detalle.
>
> **Y cada versión entrega su API Y SU PANTALLA.** No hay una versión «de
> back» y otra «de front»: se construyen en paralelo, y una versión no está
> cerrada si la API responde y la pantalla no. Ver «La estrategia» abajo.
>
> La ruta es la que define
> [modulo_investigacion.md](../../../ProyectosDeAula/docs/modulo_investigacion.md);
> aquí no se inventa nada, se ordena.

## La ruta

| Versión | Qué agrega (acumulativo) | Estado |
|---|---|---|
| **v1** | CRUD completo de las **tablas sin clave foránea**, con los catálogos del Excel cargados — **API y pantallas** | **En curso** ([spec](v1_area_conocimiento/2_spec.md)) |
| v2 | CRUD de las **10 tablas con clave foránea**: las FK como listas desplegables cargadas desde la API, y validación de integridad referencial — **API y pantallas** | Sin especificar |
| v3 | **JWT**, sesiones y control de acceso por roles; CRUD de `usuario`, `rol` y `rol_usuario` solo para administradores | Sin especificar |
| v4 | **10 consultas multitabla** (4+ tablas cada una), dashboard con gráficos, páginas corporativas, responsive/PWA y **publicación** en un servidor | Sin especificar |

## La estrategia: back y front EN PARALELO

**Cada versión entrega su parte de la API *y* su parte del front.** Conviene
decir por qué, porque la alternativa —construir toda la API y meter el front
al final— es la que uno hace por inercia.

| | |
|---|---|
| **Lo terminado se le puede mostrar a alguien** | Una versión que solo trae endpoints se sustenta con Swagger. Una que trae pantallas se le muestra a quien la pidió |
| **El contrato se ejercita de inmediato** | Uno descubre que el JSON es incómodo **cuando le toca pintarlo**. Si el front llega tres versiones después, el contrato lleva tres versiones equivocado |
| **No hay front de golpe al final** | Es el error que se paga caro: seis entidades de API esperando un front que nace con una sola |
| **Es lo que pide el curso** | `0_METODOLOGIA.md` §2, textual: *«v1 — CRUD de las tablas sin FK del módulo — **API REST + Frontend funcionando**»* |

Y lo que cuesta, que también hay que decirlo: **cada versión es el doble de
grande**, y cada compuerta revisa dos stacks. Se compensa recortando el
alcance — esta v1 toma **una** tabla, no seis.

> **La regla operativa:** una versión **no está cerrada** si la API responde y
> la pantalla no. Media versión no es una versión.

### El stack del front

**Blazor Server sobre .NET 10**, en un tercer contenedor, en el puerto
**8071**. Habla con la API **solo por HTTP**: no tiene cadena de conexión, ni
driver de base de datos, ni servicio `sqlserver` en su `depends_on`.

Que el front y la API estén los dos en C# **no cambia nada de eso**, y hay que
cuidarlo: la tentación de compartir una clase entre los dos proyectos existe
aquí y no existiría con dos lenguajes distintos. **No se comparte nada.** El
front tiene su propia clase `AreaConocimiento`, que se parece a la de la API
porque el contrato es el mismo — no porque sea la misma.

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
