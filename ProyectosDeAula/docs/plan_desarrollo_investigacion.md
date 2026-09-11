# Plan de Desarrollo de Software

## Módulo de Investigación

---

| | |
|---|---|
| **Proyecto** | Proyecto de aula — módulo de Investigación |
| **Documento** | Plan de Desarrollo de Software |
| **Versión del documento** | 1.0 |
| **Fecha** | 11 de septiembre de 2026 |
| **Alcance de la v1** | `area_conocimiento`, `objetivo_desarrollo_sostenible`, `area_aplicacion`, `termino_clave`, `universidad`, `linea_investigacion` — **con su front** |

## Historial de revisiones

| Fecha | Versión | Descripción | Autor |
|---|---|---|---|
| 11-09-2026 | 1.0 | Primera versión del plan | Carlos Arturo Castro Castro |
| \_\_\_\_/\_\_\_\_/\_\_\_\_\_\_\_\_ | 1.1 | Ajustes del equipo al cerrar la v1 | |

---

## 1. Introducción

### a. Propósito

Este documento **no describe el sistema**: describe **el trabajo**. Qué se
entrega, en qué orden, en qué semana, quién responde y con qué reglas.
El sistema está descrito en [`modulo_investigacion.md`](modulo_investigacion.md), en
[`entrevista_investigacion.md`](entrevista_investigacion.md) y en
[`historias_usuario_investigacion.md`](historias_usuario_investigacion.md).

Qué lleva un plan de desarrollo y de dónde sale su estructura está en
[`CONCEPTOS_PLAN_DE_DESARROLLO.md`](CONCEPTOS_PLAN_DE_DESARROLLO.md).

### b. Alcance

Cubre **las cuatro entregas del semestre** con sus semanas y sus
entregables, y detalla al nivel de horas **la entrega 1**. Las horas de
las entregas 2 a 4 se estiman cuando se escriban sus historias: este plan
**no inventa** estimaciones de trabajo que todavía nadie ha desglosado.

### c. Resumen

El módulo de Investigación sirve a Vicerrectoría de Investigación.
El problema que lo justifica es la cacería de correos cada vez que hay que reportar producción, y las convocatorias que se pierden por no saber quién sabe qué.
La versión 1 construye los **6 catálogos sin clave foránea** —`/api/area-conocimientos`, `/api/objetivo-desarrollo-sostenibles`…— con su
API REST y su front.

---

## 2. Vista general del proyecto

### a. Objetivos

| # | Objetivo | Cómo se sabe que se cumplió |
|---|---|---|
| 1 | Registrar y consultar los 6 catálogos del módulo | Los 6 CRUD responden con sus códigos de estado correctos |
| 2 | Mostrarlo en una pantalla usable | El listado y el formulario funcionan desde el navegador |
| 3 | No perder información | Nada se borra físicamente: `activo` en todas las tablas |
| 4 | Poder crecer | Cada entrega agrega entidades sin reescribir las anteriores |

### b. Suposiciones

| # | Suposición | Qué pasa si es falsa |
|---|---|---|
| S1 | Los datos de catálogo salen del Excel del `Mapa_conocimiento/` | Hay que sembrar datos a mano y la carga sube |
| S2 | El equipo tiene entre 3 y 5 personas | La carga por persona cambia (§4.d) |
| S3 | El stack elegido ya está aprobado por el profesor | Se pierde tiempo de la fase de inicio |
| S4 | Las personas de la entrevista son **inventadas**; el profesor hace de cliente | Las dudas del cliente se resuelven en clase, no por correo |

### c. Restricciones

Son las reglas que **ninguna decisión posterior puede violar**. Salen de
`0_METODOLOGIA.md` y de la constitución del equipo.

| # | Restricción | Por qué |
|---|---|---|
| R1 | El borrado es **lógico**, siempre | Lo retirado sigue haciendo falta en los informes |
| R2 | Toda consulta SQL va **parametrizada** | Ningún dato del cliente se concatena |
| R3 | La API es **específica por recurso** — `/api/area-conocimientos`, no `/api/{tabla}` | Una ruta genérica no se documenta ni se versiona |
| R4 | **Cada entrega incluye su front** | Una versión que el usuario no puede ver no es una versión |
| R5 | El front **no** habla con la base de datos | Es la separación que sostiene todo lo demás |
| R6 | Todo levanta con **un solo comando** | Si armarlo es difícil, nadie lo va a probar |
| R7 | **Dos repositorios privados** con `ccastro2050` invitado | Sin acceso del profesor, la entrega no existe |
| R8 | Cada estudiante en **su rama**; todo por PR; solo el encargado hace merge | La sustentación individual se respalda en los commits |

### d. Entregables

**58 entregables en las cuatro entregas**, repartidos así:

| Entrega | Documentos | Artefactos | Total |
|---|---|---|---|
| **v1** | 10 — `1_constitution`, `0_mapa_versiones`, `2_spec` … `8_tasks`, `9_checklist` | 5 | **15** |
| **v2** | 8 — `2_spec` … `8_tasks`, `9_checklist` | 5 | **13** |
| **v3** | 8 — ídem | 5 | **13** |
| **v4** | 8 — ídem | 5 + 4 de cierre | **17** |
| | | | **58** |

Los **5 artefactos** de cada entrega son los mismos:

| # | Artefacto | Cómo se verifica |
|---|---|---|
| 1 | Repositorio de la **API** con el tag `vN` en `main` | El tag existe y el profesor puede verlo |
| 2 | Repositorio del **front** con el tag `vN` | El tag existe y el front consume la API |
| 3 | **Script de la base de datos** actualizado | Levanta desde cero con un comando |
| 4 | **Las pruebas** de la entrega | Al menos una que verifique de verdad |
| 5 | **Evidencia del quickstart** pasando | Se sigue el `7_quickstart.md` en una máquina limpia |

Los **4 de cierre de la v4**: las 10 consultas multitabla expuestas como
endpoints, el dashboard con gráficos, las páginas corporativas con imagen
corporativa y diseño responsive, y **el sitio publicado con su URL**.

> La `GUIA_IA<N>.md` de cada versión es **opcional pero recomendada**, y
> por eso no se cuenta arriba. Si el equipo construye con ayuda de IA,
> el prompt se escribe ahí: el chat se cierra, la guía queda.

### e. Evolución de este plan

El plan se corrige al cerrar cada entrega, con las **horas reales** en la
mano (§4.c). Un plan que no se corrige es un plan que nadie leyó.

---

## 3. Organización del proyecto

### a. Participantes

| Rol | Quién | Qué hace |
|---|---|---|
| **Cliente** | Vicerrectoría de Investigación | Aprueba las historias y los criterios de aceptación |
| **Interesados** | Dra. Beatriz Londoño, Andrés Quintero, Luz Mery Gaviria y Sandra Pérez | Los de la entrevista: sus respuestas son el origen de las historias |
| **Equipo de desarrollo** | Los estudiantes del equipo | Escriben la especificación, construyen y responden por su rama |
| **Encargado del `main`** | Un integrante, designado por el equipo | Revisa los PR y es **el único** que hace merge |
| **Revisor** | El profesor del curso | Revisa la especificación y el resultado; califica |

### b. Interfaces externas

- **GitHub**: dos repositorios privados con `ccastro2050` invitado.
- **El Excel del `Mapa_conocimiento/`**: la fuente de los datos de catálogo.
- **El profesor**, que hace de cliente cuando hay que resolver una duda
  que la entrevista dejó abierta.

### c. Roles y responsabilidades

| Responsabilidad | De quién es |
|---|---|
| Escribir la especificación **antes** del código | Del equipo |
| Marcar lo que el material no dice, con `[NECESITA ACLARACIÓN]` | De quien lo encuentre |
| Resolver la contradicción del cliente y dejarla escrita en el `2_spec.md` | Del equipo, en la compuerta 1 |
| Revisar con la lista de chequeo **la parte que escribió otro** | De cada integrante |
| Firmar el `9_checklist.md` | De **una persona**, no de un guion |
| Hacer merge a `main` y poner el tag `vN` | Del encargado del `main` |
| Aceptar o rechazar una entrega | Del revisor |

---

## 4. Gestión del proceso

### a. Plan de las fases

Las cuatro fases —inicio, elaboración, construcción, transición— y la idea
de que **cada una termina en un hito verificable, no en una fecha**, están
explicadas en [`CONCEPTOS_PLAN_DE_DESARROLLO.md`](CONCEPTOS_PLAN_DE_DESARROLLO.md).

| Fase | Qué pasa | Termina cuando |
|---|---|---|
| **Inicio** | Se lee el módulo, la entrevista y las historias; se marcan las dudas | No queda ningún `[NECESITA ACLARACIÓN]` sin resolver |
| **Elaboración** | Se escribe el spec kit de la entrega y se revisa contra la constitución | El chequeo de constitución pasa |
| **Construcción** | Se programa la API, el front y las pruebas | Los criterios de aceptación se cumplen, uno por uno |
| **Transición** | Se cierra: quickstart probado desde cero, checklist firmado y tag `vN` | El `9_checklist.md` está firmado |

**Las tres compuertas** son lo que separa una fase de la siguiente:

1. Antes de programar: ningún `[NECESITA ACLARACIÓN]` sin resolver.
2. Antes de programar: chequeo contra la constitución.
3. Al cerrar: lista de chequeo firmada por una persona.

**Las cuatro fases se repiten en cada una de las cuatro entregas.** No son
cuatro fases repartidas entre las cuatro entregas: cada entrega las recorre
completas, porque cada una tiene su propio spec kit y su propio cierre.

### b. Calendario: las cuatro entregas

**Las semanas de entrega son las de `0_METODOLOGIA.md` §2.1** —no se
inventan aquí—. La **fecha exacta la fija el profesor en clase** y se
anota en el espacio en blanco.

| Entrega | Semana de entrega | Fecha exacta (su grupo) | Qué agrega | Entregables | Peso |
|---|---|---|---|---|---|
| **Entrega 1** (`v1`) | Última semana de **septiembre** | \_\_\_\_/\_\_\_\_/\_\_\_\_\_\_\_\_ | CRUD de las tablas **sin clave foránea** del módulo — API REST + front | **15** | 20 % |
| **Entrega 2** (`v2`) | Última semana de **octubre** | \_\_\_\_/\_\_\_\_/\_\_\_\_\_\_\_\_ | CRUD de **todas** las tablas: FK con listas desplegables y tablas puente | **13** | 20 % |
| **Entrega 3** (`v3`) | Segunda semana de **noviembre** | \_\_\_\_/\_\_\_\_/\_\_\_\_\_\_\_\_ | JWT, sesiones y control de acceso por roles; CRUD de usuario/rol | **13** | 20 % |
| **Entrega 4** (`v4`) | Última semana de **noviembre** | \_\_\_\_/\_\_\_\_/\_\_\_\_\_\_\_\_ | 10 consultas multitabla, dashboard, imagen corporativa y publicación | **17** | 20 % |
| | | | | **58** | **80 %** |

El 20 % restante del semestre es la **evaluación individual
teórico-práctica** de la segunda semana de septiembre, que no entrega
artefactos de este proyecto.

De cada 20 % de entrega, **10 % es sustentación individual** —respaldada
por los commits de su rama— y **10 % es la entrega en equipo**.

> **Una entrega cerrada no se reabre**: los ajustes van en la siguiente. Y
> al cerrar cada una, los criterios de **todas las anteriores** deben
> seguir pasando: las entregas son acumulativas.

### c. Cronograma de la entrega 1

Las horas de construcción **no son inventadas aquí**: son las estimadas
historia por historia en
[`historias_usuario_investigacion.md`](historias_usuario_investigacion.md).

**La columna de horas reales se llena mientras se trabaja.** Es la que
enseña: nadie estima bien al principio, y lo que se aprende es cuánto se
equivoca uno y en qué dirección.

| # | Entregable | Fase | Est. | Reales | Responsable |
|---|---|---|---|---|---|
| 0.1 | Leer el módulo, la entrevista y las historias; marcar las dudas | Inicio | 2 | | |
| 0.2 | Resolver la contradicción del cliente y las 3 preguntas abiertas | Inicio | 2 | | |
| 0.3 | `1_constitution.md` y `0_mapa_versiones.md` | Elaboración | 3 | | |
| 0.4 | `2_spec.md` con sus clarificaciones | Elaboración | 3 | | |
| 0.5 | `3_plan.md` + chequeo de constitución | Elaboración | 2 | | |
| 0.6 | `4_research.md`, `5_data_model.md`, `6_contracts.md` | Elaboración | 3 | | |
| 0.7 | `7_quickstart.md` y `8_tasks.md` | Elaboración | 1 | | |
| 1 | Mantener el catálogo de áreas de conocimiento | Construcción | 8 | | |
| 2 | Consultar los ODS sin poder inventarlos | Construcción | 5 | | |
| 3 | Registrar las áreas de aplicación | Construcción | 5 | | |
| 4 | Registrar términos clave con su traducción | Construcción | 8 | | |
| 5 | Registrar y corregir líneas de investigación | Construcción | 8 | | |
| 6 | Retirar una línea abandonada | Construcción | 6 | | |
| 7 | Mantener las sedes | Construcción | 5 | | |
| 8 | Que un listado largo no tumbe la pantalla | Construcción | 3 | | |
| 9 | Que los datos no se corrompan | Construcción | 8 | | |
| 10 | Ver y editar desde una pantalla | Construcción | 14 | | |
| 9.1 | `9_checklist.md` revisado y **firmado** | Transición | 2 | | |
| 9.2 | Quickstart probado desde cero en máquina limpia | Transición | 2 | | |
| 9.3 | Merge a `main`, tag `v1` y entrega de los enlaces | Transición | 2 | | |
| | **Total** | | **92** | | |

| Fase | Horas | % |
|---|---|---|
| Inicio | 4 | 4 % |
| Elaboración | 12 | 13 % |
| Construcción | 70 | 76 % |
| Transición | 6 | 7 % |
| **Total** | **92** | **100 %** |

Son **27 puntos de historia** en 92 horas de equipo.

### d. Carga por persona

Las 92 horas son **de equipo**. Desde hoy hasta la última semana de
septiembre hay alrededor de **diez días hábiles**:

| Integrantes | Horas por persona | Por día hábil |
|---|---|---|
| 3 | 30.7 | 3.1 |
| 4 | 23.0 | 2.3 |
| 5 | 18.4 | 1.8 |

Si al equipo le salen **más de tres horas diarias por persona**, el
problema no es de esfuerzo: es que la entrega 1 tiene más alcance del que
cabe, y eso se habla con el profesor **ahora**, no la noche anterior.

### e. Seguimiento y control

El seguimiento **es el historial de Git**, y en este curso es además
criterio de calificación:

- **Un commit por cada fase** del `8_tasks.md`, con su comprobación hecha.
- Cuando algo falla, el commit que lo arregla **dice qué falló, qué
  respondió el sistema y qué lo causaba**.
- Cada estudiante en **su rama**; todo entra a `main` por PR; el tag `vN`
  lo pone el encargado cuando los criterios pasan.
- **Lo que se mide no es cuántos commits hay, sino cómo están repartidos
  en el tiempo.** Dieciséis commits en tres minutos son un commit.

**El control de calidad** son los criterios de aceptación de las historias:
cada uno es una afirmación que se puede probar. Si un criterio no se puede
probar, está mal escrito y se devuelve.

---

## 5. Referencias

1. `0_METODOLOGIA.md` — el calendario de las cuatro entregas (§2.1), qué
   agrega cada una (§2), las tres compuertas (§3.1) y la rúbrica (§7).
2. [`CONCEPTOS_PLAN_DE_DESARROLLO.md`](CONCEPTOS_PLAN_DE_DESARROLLO.md) —
   de dónde sale la estructura de este documento (ISO/IEC/IEEE 16326:2019)
   y las cuatro fases (Kruchten, RUP), con sus fuentes verificables.
3. [`CONCEPTOS_HISTORIAS_DE_USUARIO.md`](CONCEPTOS_HISTORIAS_DE_USUARIO.md)
   — INVEST, las tres C y cómo se parte una historia.
4. [`CONCEPTOS_ELICITACION.md`](CONCEPTOS_ELICITACION.md) — cómo se
   conduce la entrevista de la que salieron las historias.
5. [`modulo_investigacion.md`](modulo_investigacion.md) — el modelo de datos y los
   conteos de registros por tabla.
