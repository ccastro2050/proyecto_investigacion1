# Historias de usuario — Módulo de Mapa de Conocimiento

**Versión 1 · Las ocho tablas sin claves foráneas**

| | |
|---|---|
| **Módulo** | Mapa de Conocimiento — ver [`modulo_mapa_conocimiento.md`](modulo_mapa_conocimiento.md) |
| **De dónde salen** | [`entrevista_mapa_conocimiento.md`](entrevista_mapa_conocimiento.md) |
| **Alcance** | `area_conocimiento`, `objetivo_desarrollo_sostenible`, `area_aplicacion`, `termino_clave`, `linea_investigacion`, `aliado`, `tipo_producto`, `proyecto` — **con su front** |

---

## Historia 1 — Registrar un proyecto aprobado

| | | |
|---|---|---|
| **Usuario:** Marcela Ospina — coordinadora de proyectos | **Prioridad:** Alta | **Riesgo:** Medio |
| **Puntos:** 5 | **Horas:** 12 | **Versión:** v1 |

**Yo, Marcela Ospina, quiero registrar un proyecto aprobado con su
presupuesto y sus fechas, para que la vicerrectoría pueda ver cuánta plata
está comprometida sin que yo tenga que sumar tres archivos.**

**Criterios de aceptación:**

1. Título, resumen, presupuesto, fecha de inicio, tipo de financiación y
   tipo de fondos son obligatorios: si falta alguno, responde **422** e
   indica cuál.
2. **El presupuesto tiene que ser mayor que cero**: un proyecto con
   presupuesto cero no se aprobó, es un error de digitación.
3. **La fecha de fin no puede ser anterior a la de inicio**: si lo es,
   responde **422**.
4. Un proyecto se registra **aunque todavía no tenga investigadores,
   aliados ni productos asociados**.
5. El listado devuelve los proyectos **activos** y dice cuántos hay.

---

## Historia 2 — Corregir un proyecto sin repetir todos los campos

| | | |
|---|---|---|
| **Usuario:** Marcela Ospina — coordinadora de proyectos | **Prioridad:** Alta | **Riesgo:** Medio |
| **Puntos:** 3 | **Horas:** 8 | **Versión:** v1 |

**Yo, Marcela Ospina, quiero corregir la fecha de fin de un proyecto sin
volver a escribir el resto, porque las fechas son justamente lo que más
se equivoca.**

**Criterios de aceptación:**

1. El **reemplazo completo** exige todos los campos obligatorios: si falta
   uno, **422**.
2. La **corrección parcial** acepta un cuerpo incompleto y responde
   **200**, cambiando solo lo que llegó.
3. Los campos que no llegaron **conservan su valor anterior**.
4. La regla de las fechas **se sigue aplicando en la corrección parcial**:
   si la nueva fecha de fin queda antes del inicio guardado, **422**.
5. Corregir un proyecto que no existe responde **404** en los dos casos.

---

## Historia 3 — Retirar un proyecto cancelado

| | | |
|---|---|---|
| **Usuario:** Marcela Ospina — coordinadora de proyectos | **Prioridad:** Media | **Riesgo:** Alto |
| **Puntos:** 3 | **Horas:** 6 | **Versión:** v1 |

**Yo, Marcela Ospina, quiero retirar del listado un proyecto que se
canceló, para que no aparezca en los informes de proyectos activos ni
sume presupuesto comprometido.**

**Criterios de aceptación:**

1. Al retirarlo, deja de aparecer en el listado.
2. Consultarlo directamente después responde **404**.
3. Retirarlo dos veces responde **404** la segunda vez.
4. **Los productos que ese proyecto alcanzó a generar siguen contando**
   para la clasificación del grupo: un proyecto cancelado no borra lo que
   ya produjo.

> *Los criterios 1 a 3 son de Marcela; el 4 es la objeción de Beatriz.*

---

## Historia 4 — Mantener la tipología de productos de Minciencias

| | | |
|---|---|---|
| **Usuario:** Sandra Pérez — analista de investigación | **Prioridad:** Alta | **Riesgo:** Medio |
| **Puntos:** 3 | **Horas:** 8 | **Versión:** v1 |

**Yo, Sandra Pérez, quiero mantener la tipología oficial de productos con
su categoría, clase, nombre y tipología, para que el reporte a Minciencias
no se caiga por una clasificación inventada.**

**Criterios de aceptación:**

1. Categoría, clase, nombre y tipología son obligatorios: sin alguno,
   **422** indicando cuál.
2. El listado devuelve los **activos** y dice cuántos hay.
3. Consultar uno inexistente o inactivo responde **404**.
4. La corrección parcial permite cambiar solo el nombre y responde
   **200**.

> *De la ronda 2: «esa lista no la inventamos nosotros, viene de
> Minciencias, y si la cambiamos por nuestra cuenta, el reporte se cae».*

---

## Historia 5 — Registrar los aliados por su NIT

| | | |
|---|---|---|
| **Usuario:** Marcela Ospina — coordinadora de proyectos | **Prioridad:** Alta | **Riesgo:** Medio |
| **Puntos:** 3 | **Horas:** 7 | **Versión:** v1 |

**Yo, Marcela Ospina, quiero registrar las entidades aliadas con su NIT,
razón social, contacto y ciudad, para poder decir con quién hicimos cada
proyecto.**

**Criterios de aceptación:**

1. NIT, razón social y ciudad son obligatorios: sin alguno, **422**.
2. **El NIT no puede repetirse**: identifica a la entidad, y dos veces el
   mismo NIT es la misma entidad dos veces.
3. El listado devuelve solo los aliados **activos**.
4. Un aliado se registra **aunque todavía no tenga proyectos**.

---

## Historia 6 — Mantener los catálogos compartidos

| | | |
|---|---|---|
| **Usuario:** Sandra Pérez — analista de investigación | **Prioridad:** Alta | **Riesgo:** Bajo |
| **Puntos:** 5 | **Horas:** 12 | **Versión:** v1 |

**Yo, Sandra Pérez, quiero mantener las áreas de conocimiento, los ODS,
las áreas de aplicación y las líneas de investigación, para clasificar
cada proyecto con la misma lista que usa el resto de la universidad.**

**Criterios de aceptación:**

1. Son **cuatro recursos distintos**: `/api/areas-conocimiento`,
   `/api/objetivos-desarrollo-sostenible`, `/api/areas-aplicacion` y
   `/api/lineas-investigacion`.
2. En áreas de conocimiento, **gran área, área y disciplina** son
   obligatorias: sin alguna, **422** indicando cuál.
3. En los ODS, **el nombre y la categoría** son obligatorios.
4. En líneas de investigación, **nombre y descripción** son obligatorios,
   y una línea se registra **aunque todavía no tenga proyectos
   adscritos**.
5. Cada listado devuelve solo los **activos** y dice cuántos hay.
6. Si un catálogo no tiene ningún registro activo, responde **204**, no
   una lista vacía.

---

## Historia 7 — Registrar términos clave con su traducción

| | | |
|---|---|---|
| **Usuario:** Prof. Rubén Darío Agudelo — investigador | **Prioridad:** Media | **Riesgo:** Medio |
| **Puntos:** 2 | **Horas:** 5 | **Versión:** v1 |

**Yo, Rubén Darío Agudelo, quiero registrar los términos clave con su
traducción al inglés, para que nuestros productos aparezcan en las
búsquedas internacionales.**

**Criterios de aceptación:**

1. El término y su traducción son obligatorios: sin alguno, **422**.
2. **No puede haber dos términos iguales**: el término es el
   identificador.
3. El listado devuelve solo los **activos**.

---

## Historia 8 — Que un listado largo no tumbe la pantalla

| | | |
|---|---|---|
| **Usuario:** Sandra Pérez — analista de investigación | **Prioridad:** Media | **Riesgo:** Bajo |
| **Puntos:** 1 | **Horas:** 3 | **Versión:** v1 |

**Yo, Sandra Pérez, quiero pedir solo un pedazo de cualquier listado y
saber cuántos registros hay en total, para no cargar cientos de proyectos
cuando necesito diez.**

**Criterios de aceptación:**

1. Los ocho listados aceptan un parámetro de límite.
2. La respuesta dice **cuántos** registros trae y **cuál límite** se
   aplicó.
3. Si el límite no es un número, responde **422**.
4. Sin límite, se aplica uno por defecto y se informa cuál.

---

## Historia 9 — Que los datos no se corrompan

| | | |
|---|---|---|
| **Usuario:** Dra. Beatriz Londoño — vicerrectora de Investigación | **Prioridad:** Alta | **Riesgo:** Alto |
| **Puntos:** 3 | **Horas:** 8 | **Versión:** v1 |

**Yo, Beatriz Londoño, necesito que el sistema no acepte datos que dejen
la base inconsistente, porque de aquí sale el reporte a Minciencias y de
ese reporte depende la clasificación de los grupos.**

**Criterios de aceptación:**

1. Las consultas van **parametrizadas**: ningún dato del cliente se
   concatena.
2. Dos identificadores iguales en la misma tabla no se pueden guardar.
3. Un campo obligatorio vacío se rechaza con **422**, no se guarda como
   cadena vacía.
4. El error dice **qué campo** está mal, sin exponer el mensaje del motor
   de base de datos.

---

## Historia 10 — Ver y editar desde una pantalla

| | | |
|---|---|---|
| **Usuario:** Marcela Ospina — coordinadora de proyectos | **Prioridad:** Alta | **Riesgo:** Medio |
| **Puntos:** 5 | **Horas:** 14 | **Versión:** v1 |

**Yo, Marcela Ospina, quiero una pantalla donde ver y editar los ocho
catálogos, para que la vicerrectoría vea el presupuesto comprometido sin
pedirme un informe.**

**Criterios de aceptación:**

1. Hay un listado por cada una de las ocho tablas, con sus columnas.
2. Hay formulario para crear y editar, y los errores del servidor se
   muestran junto al campo, en palabras entendibles.
3. La pantalla llama a la API **por HTTP**: no se conecta a la base.
4. **Con la API apagada, la pantalla abre igual** y avisa.
5. Corre en **su propio contenedor** y sube con el mismo comando.

---

## Resumen de la iteración

| # | Historia | Puntos | Horas |
|---|---|---|---|
| 1 | Registrar un proyecto aprobado | 5 | 12 |
| 2 | Corregir un proyecto sin repetir todos los campos | 3 | 8 |
| 3 | Retirar un proyecto cancelado | 3 | 6 |
| 4 | Mantener la tipología de productos de Minciencias | 3 | 8 |
| 5 | Registrar los aliados por su NIT | 3 | 7 |
| 6 | Mantener los catálogos compartidos | 5 | 12 |
| 7 | Registrar términos clave con su traducción | 2 | 5 |
| 8 | Que un listado largo no tumbe la pantalla | 1 | 3 |
| 9 | Que los datos no se corrompan | 3 | 8 |
| 10 | Ver y editar desde una pantalla | 5 | 14 |
| | **Total** | **33** | **83** |

---

## Trazabilidad

| Historia | Sale de |
|---|---|
| 1 | Ronda 3 y 4: «sumar lo comprometido me toma un día entero»; presupuesto mayor que cero y fechas en orden |
| 2 | Ronda 3: «las fechas son lo que más se equivoca» |
| 3 | Ronda 3: Marcela elimina; Beatriz exige que los productos generados sigan contando |
| 4 | Ronda 2: la tipología de Minciencias, que no se inventa |
| 5 | Ronda 4: «el NIT no se repite: identifica a la entidad» |
| 6 | Ronda 2: los catálogos compartidos con los otros módulos |
| 7 | Ronda 4: «sin inglés, el producto no aparece en las búsquedas internacionales» |
| 8 | Ronda 4: pedir un pedazo y saber cuántos hay |
| 9 | Ronda 4: «que lo rechace y diga cuál campo» |
| 10 | Ronda 1 y 4: ver en una pantalla cuánto presupuesto está comprometido |

**El «para qué» de todas** sale de la ronda 1: la cacería de correos antes
de cada reporte a Minciencias, y la clasificación de grupos que de eso
depende.
