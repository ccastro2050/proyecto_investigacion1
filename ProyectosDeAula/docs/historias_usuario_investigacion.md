# Historias de usuario — Módulo de Investigación

**Versión 1 · Las seis tablas sin claves foráneas**

| | |
|---|---|
| **Módulo** | Investigación — ver [`modulo_investigacion.md`](modulo_investigacion.md) |
| **De dónde salen** | [`entrevista_investigacion.md`](entrevista_investigacion.md) |
| **Alcance** | `area_conocimiento`, `objetivo_desarrollo_sostenible`, `area_aplicacion`, `termino_clave`, `universidad`, `linea_investigacion` — **con su front** |

> Cada historia se rastrea hasta una frase de la entrevista. La tabla del
> final dice cuál. Las personas son inventadas.

---

## Historia 1 — Mantener el catálogo de áreas de conocimiento

| | | |
|---|---|---|
| **Usuario:** Sandra Pérez — analista de la oficina de investigación | **Prioridad:** Alta | **Riesgo:** Bajo |
| **Puntos:** 3 | **Horas:** 8 | **Versión:** v1 |

**Yo, Sandra Pérez, como analista, quiero registrar y corregir las áreas
de conocimiento con sus tres niveles, para que todo lo que se clasifique
en la universidad use la misma lista.**

**Criterios de aceptación:**

1. Cada área tiene **gran área, área y disciplina**: si falta alguno, el
   sistema responde **422** e indica cuál campo falta.
2. El listado devuelve las áreas **activas**, y dice **cuántas** hay en
   total.
3. Consultar un área que no existe, o inactiva, responde **404**.
4. El **reemplazo completo** exige los tres niveles; la **corrección
   parcial** acepta cambiar solo la disciplina y responde **200**.

> *De la ronda 4: «los tres niveles, siempre. Un área sin disciplina no
> sirve para clasificar nada», y el ejemplo de Ingeniería de Sistemas.*

---

## Historia 2 — Consultar los ODS sin poder inventarlos

| | | |
|---|---|---|
| **Usuario:** Sandra Pérez — analista | **Prioridad:** Media | **Riesgo:** Bajo |
| **Puntos:** 2 | **Horas:** 5 | **Versión:** v1 |

**Yo, Sandra Pérez, quiero consultar los diecisiete Objetivos de
Desarrollo Sostenible con su categoría, para clasificar las líneas sin
depender de que alguien los transcriba cada vez.**

**Criterios de aceptación:**

1. El listado devuelve los ODS con su número, nombre y **categoría**:
   social, económica o ambiental.
2. La categoría solo acepta esos tres valores.
3. Consultar uno que no existe responde **404**.
4. Si el catálogo está vacío, responde **204**, no una lista vacía.

> *De la ronda 4: «son diecisiete… esos no se inventan ni se agregan».*
> **Ojo:** la entrevista dejó abierto si el sistema debe permitir
> corregirlos. Eso se marca como `[NECESITA ACLARACIÓN]`.

---

## Historia 3 — Registrar las áreas de aplicación

| | | |
|---|---|---|
| **Usuario:** Sandra Pérez — analista | **Prioridad:** Media | **Riesgo:** Bajo |
| **Puntos:** 2 | **Horas:** 5 | **Versión:** v1 |

**Yo, Sandra Pérez, quiero mantener la lista de sectores económicos donde
se aplica el conocimiento, para poder decir en qué sector impacta cada
línea de investigación.**

**Criterios de aceptación:**

1. Crear un área de aplicación **sin nombre** responde **422**.
2. El listado devuelve solo las **activas**.
3. Se puede corregir el nombre sin afectar nada de lo que ya la usa.

---

## Historia 4 — Registrar términos clave con su traducción

| | | |
|---|---|---|
| **Usuario:** Prof. Luz Mery Gaviria — líder de grupo | **Prioridad:** Alta | **Riesgo:** Medio |
| **Puntos:** 3 | **Horas:** 8 | **Versión:** v1 |

**Yo, Luz Mery Gaviria, como líder de grupo, quiero registrar un término
clave con su traducción al inglés, para que lo que publicamos aparezca en
las búsquedas internacionales.**

**Criterios de aceptación:**

1. El término y su traducción son **obligatorios**: si falta alguno,
   responde **422** e indica cuál.
2. **No se pueden registrar dos términos iguales**: el término es el
   identificador, y el segundo intento se rechaza.
3. El listado devuelve solo los **activos**; si no hay ninguno, **204**.
4. Consultar un término retirado responde **404**.

> *De la ronda 2 y la 4: «uno escribe machine learning, otro aprendizaje
> de máquina, y después nadie encuentra nada».*

---

## Historia 5 — Registrar y corregir líneas de investigación

| | | |
|---|---|---|
| **Usuario:** Prof. Luz Mery Gaviria — líder de grupo | **Prioridad:** Alta | **Riesgo:** Medio |
| **Puntos:** 3 | **Horas:** 8 | **Versión:** v1 |

**Yo, Luz Mery Gaviria, quiero registrar una línea de investigación con su
nombre y su descripción, para que el grupo defina en qué trabaja sin
esperar a que la administración lo autorice.**

**Criterios de aceptación:**

1. Nombre y descripción son obligatorios: sin alguno, **422**.
2. Una línea se registra **aunque todavía no tenga grupos ni semilleros**.
3. La **corrección parcial** permite cambiar solo la descripción y
   responde **200**; el **reemplazo completo** exige los dos campos y
   responde **422** si falta uno.
4. Consultar una línea que no existe responde **404**.

> *De la ronda 1: «si el sistema me impone una lista cerrada de líneas, no
> lo vamos a usar».*

---

## Historia 6 — Retirar una línea abandonada

| | | |
|---|---|---|
| **Usuario:** Andrés Quintero — coordinador de semilleros | **Prioridad:** Media | **Riesgo:** Alto |
| **Puntos:** 3 | **Horas:** 6 | **Versión:** v1 |

**Yo, Andrés Quintero, como coordinador, quiero retirar una línea de
investigación que se abandonó, para que nadie la escoja al crear un
semillero nuevo.**

**Criterios de aceptación:**

1. Al retirarla, deja de aparecer en el listado.
2. Consultarla directamente después responde **404**.
3. Retirarla dos veces responde **404** la segunda vez.
4. **Los productos asociados a esa línea siguen apareciendo en los
   informes históricos.**

> *Los criterios 1 a 3 son de Andrés; el 4 es la objeción de Beatriz. Las
> dos peticiones, literalmente, no pueden ser ciertas a la vez.*

---

## Historia 7 — Mantener las sedes

| | | |
|---|---|---|
| **Usuario:** Sandra Pérez — analista | **Prioridad:** Media | **Riesgo:** Bajo |
| **Puntos:** 2 | **Horas:** 5 | **Versión:** v1 |

**Yo, Sandra Pérez, quiero registrar las sedes y seccionales con su
ciudad, para poder decir cuántas líneas activas hay en cada una.**

**Criterios de aceptación:**

1. Nombre, **tipo** —sede o seccional— y ciudad son obligatorios.
2. El tipo solo acepta esos dos valores.
3. Corregir el nombre de una sede **no afecta** lo que cuelga de ella.

---

## Historia 8 — Que un listado largo no tumbe la pantalla

| | | |
|---|---|---|
| **Usuario:** Sandra Pérez — analista | **Prioridad:** Media | **Riesgo:** Bajo |
| **Puntos:** 1 | **Horas:** 3 | **Versión:** v1 |

**Yo, Sandra Pérez, quiero pedir solo los primeros N registros de
cualquier listado, para no esperar doscientas dieciocho filas cuando
necesito diez.**

**Criterios de aceptación:**

1. Los seis listados aceptan un parámetro de límite.
2. La respuesta dice **cuántos** registros trae y **cuál límite** se
   aplicó.
3. Si el límite no es un número, responde **422**.
4. Sin límite, se aplica uno por defecto y se informa cuál.

---

## Historia 9 — Que los datos no se corrompan

| | | |
|---|---|---|
| **Usuario:** Dra. Beatriz Londoño — vicerrectora | **Prioridad:** Alta | **Riesgo:** Alto |
| **Puntos:** 3 | **Horas:** 8 | **Versión:** v1 |

**Yo, Beatriz Londoño, como vicerrectora, necesito que el sistema no
acepte datos que dejen la base inconsistente, porque con esto se responde
al Ministerio.**

**Criterios de aceptación:**

1. Ningún dato del cliente se concatena a una consulta SQL: las consultas
   van **parametrizadas**.
2. Dos identificadores iguales en la misma tabla no se pueden guardar.
3. Un campo obligatorio vacío se rechaza con **422**, no se guarda como
   cadena vacía.
4. El error dice **qué campo** está mal, sin exponer el mensaje interno
   del motor de base de datos.

---

## Historia 10 — Ver y editar desde una pantalla

| | | |
|---|---|---|
| **Usuario:** Sandra Pérez — analista | **Prioridad:** Alta | **Riesgo:** Medio |
| **Puntos:** 5 | **Horas:** 14 | **Versión:** v1 |

**Yo, Sandra Pérez, quiero una pantalla donde ver y editar los seis
catálogos, para no depender de que alguien me arme las peticiones a
mano.**

**Criterios de aceptación:**

1. Hay un listado por cada una de las seis tablas, con sus columnas.
2. Hay formulario para crear y editar, y los errores del servidor se
   muestran junto al campo, en palabras entendibles.
3. La pantalla llama a la API **por HTTP**: no se conecta a la base, no
   tiene el driver ni las credenciales.
4. **Con la API apagada, la pantalla abre igual** y avisa que el servicio
   no está disponible.
5. Corre en **su propio contenedor** y sube con el mismo
   `docker compose up -d --build`.

---

## Resumen de la iteración

| # | Historia | Puntos | Horas |
|---|---|---|---|
| 1 | Mantener el catálogo de áreas de conocimiento | 3 | 8 |
| 2 | Consultar los ODS sin poder inventarlos | 2 | 5 |
| 3 | Registrar las áreas de aplicación | 2 | 5 |
| 4 | Registrar términos clave con su traducción | 3 | 8 |
| 5 | Registrar y corregir líneas de investigación | 3 | 8 |
| 6 | Retirar una línea abandonada | 3 | 6 |
| 7 | Mantener las sedes | 2 | 5 |
| 8 | Que un listado largo no tumbe la pantalla | 1 | 3 |
| 9 | Que los datos no se corrompan | 3 | 8 |
| 10 | Ver y editar desde una pantalla | 5 | 14 |
| | **Total** | **27** | **70** |

---

## Trazabilidad

| Historia | Sale de |
|---|---|
| 1 | Ronda 4: los tres niveles; el ejemplo de Ingeniería de Sistemas |
| 2 | Ronda 2 y 4: «vienen de afuera, del OCDE y de Naciones Unidas» |
| 3 | Ronda 2: la lista de catálogos que mantiene Sandra |
| 4 | Ronda 2 y 4: «machine learning» contra «aprendizaje de máquina» |
| 5 | Ronda 1 y 3: «la propone el grupo, no la administración» |
| 6 | Ronda 3: Andrés borra; Beatriz exige el histórico |
| 7 | Ronda 4: nombre, tipo y ciudad |
| 8 | Ronda 4: «218 áreas en una sola pantalla no las lee nadie» |
| 9 | Ronda 4: «que lo rechace y diga cuál campo» |
| 10 | Ronda 3: Sandra pierde tiempo buscando |

**El «para qué» de todas** sale de la ronda 1: el informe de capacidades
que hoy toma tres semanas y debería salir el mismo día.

> **Cómo se revisan:** con INVEST, en
> [`CONCEPTOS_HISTORIAS_DE_USUARIO.md`](CONCEPTOS_HISTORIAS_DE_USUARIO.md).
> Y **estas historias no son intocables**: si su equipo encuentra que una
> está mal partida o que le falta un criterio, corríjala — y deje dicho en
> el `4_research.md` por qué.
