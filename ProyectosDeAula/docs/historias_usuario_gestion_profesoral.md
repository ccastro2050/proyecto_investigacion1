# Historias de usuario — Módulo de Gestión Profesoral

**Versión 1 · Las cinco tablas sin claves foráneas**

| | |
|---|---|
| **Módulo** | Gestión Profesoral — ver [`modulo_gestion_profesoral.md`](modulo_gestion_profesoral.md) |
| **De dónde salen** | [`entrevista_gestion_profesoral.md`](entrevista_gestion_profesoral.md) |
| **Alcance** | `area_conocimiento`, `termino_clave`, `linea_investigacion`, `programa`, `red` — **con su front** |

---

## Historia 1 — Registrar los programas académicos

| | | |
|---|---|---|
| **Usuario:** Claudia Mesa — analista de docencia | **Prioridad:** Alta | **Riesgo:** Medio |
| **Puntos:** 5 | **Horas:** 12 | **Versión:** v1 |

**Yo, Claudia Mesa, quiero registrar un programa académico con su tipo, su
nivel, su ciudad y su facultad, para que los docentes se vinculen a un
programa que existe una sola vez y no a tres versiones del mismo
nombre.**

**Criterios de aceptación:**

1. Nombre, tipo, nivel, ciudad y facultad son obligatorios: si falta
   alguno, responde **422** e indica cuál.
2. El tipo solo acepta `pregrado`, `especializacion`, `maestria` o
   `doctorado`.
3. **La fecha de cierre no puede ser anterior a la de inicio**: si lo es,
   responde **422**.
4. Cohortes y graduados son números enteros mayores o iguales a cero.
5. El listado devuelve los programas **activos** y dice cuántos hay.

---

## Historia 2 — Corregir un programa

| | | |
|---|---|---|
| **Usuario:** Claudia Mesa — analista de docencia | **Prioridad:** Alta | **Riesgo:** Medio |
| **Puntos:** 3 | **Horas:** 8 | **Versión:** v1 |

**Yo, Claudia Mesa, quiero actualizar el número de graduados de un
programa sin volver a escribir los otros ocho campos, porque eso cambia
cada semestre.**

**Criterios de aceptación:**

1. El **reemplazo completo** exige todos los campos obligatorios: si falta
   uno, **422**.
2. La **corrección parcial** acepta el mismo cuerpo incompleto y responde
   **200**, cambiando solo lo que llegó.
3. Los campos que no llegaron **conservan su valor anterior**.
4. Corregir un programa que no existe responde **404** en los dos casos.

---

## Historia 3 — Retirar un programa que se cerró

| | | |
|---|---|---|
| **Usuario:** Mauricio Álvarez — jefe de talento docente | **Prioridad:** Media | **Riesgo:** Alto |
| **Puntos:** 3 | **Horas:** 6 | **Versión:** v1 |

**Yo, Mauricio Álvarez, quiero retirar del listado un programa que dejó de
ofertarse, para que nadie vincule docentes nuevos a él.**

**Criterios de aceptación:**

1. Al retirarlo, deja de aparecer en el listado.
2. Consultarlo directamente después responde **404**.
3. Retirarlo dos veces responde **404** la segunda vez.
4. **Los graduados de ese programa y la experiencia de quienes dictaron
   ahí siguen apareciendo en los informes**, porque los pide la agencia
   acreditadora.

> *Los criterios 1 a 3 son de Mauricio; el 4 es la objeción de Patricia.*

---

## Historia 4 — Registrar las redes académicas

| | | |
|---|---|---|
| **Usuario:** Prof. Hernán Correa — docente de planta | **Prioridad:** Alta | **Riesgo:** Bajo |
| **Puntos:** 2 | **Horas:** 5 | **Versión:** v1 |

**Yo, Hernán Correa, como docente, quiero registrar las redes académicas a
las que pertenezco, para que eso cuente en la acreditación en vez de
quedarse en mi memoria.**

**Criterios de aceptación:**

1. El **nombre** y el **país** son obligatorios: sin alguno, **422**.
2. La **dirección web es opcional**: hay redes viejas que no tienen.
3. El listado devuelve solo las redes **activas**.
4. Consultar una red retirada responde **404**.

> *De la ronda 2 y 4: «la dirección web es deseable pero a veces no
> existe».*

---

## Historia 5 — Mantener el catálogo de áreas de conocimiento

| | | |
|---|---|---|
| **Usuario:** Claudia Mesa — analista de docencia | **Prioridad:** Alta | **Riesgo:** Bajo |
| **Puntos:** 3 | **Horas:** 8 | **Versión:** v1 |

**Yo, Claudia Mesa, quiero mantener las áreas de conocimiento con sus tres
niveles, para poder decir en qué área encaja el título de cada docente.**

**Criterios de aceptación:**

1. Gran área, área y disciplina son obligatorias: si falta alguna, **422**
   indicando cuál.
2. El listado devuelve las **activas** y dice cuántas hay en total.
3. Consultar un área inexistente o inactiva responde **404**.
4. Si no hay ninguna activa, responde **204**, no una lista vacía.

---

## Historia 6 — Registrar términos clave con su traducción

| | | |
|---|---|---|
| **Usuario:** Prof. Hernán Correa — docente | **Prioridad:** Media | **Riesgo:** Medio |
| **Puntos:** 3 | **Horas:** 7 | **Versión:** v1 |

**Yo, Hernán Correa, quiero registrar mis intereses de investigación como
términos clave con su traducción al inglés, para que me encuentren cuando
busquen un experto en el tema.**

**Criterios de aceptación:**

1. El término y su traducción son obligatorios: sin alguno, **422**.
2. **No puede haber dos términos iguales**: el término es el
   identificador.
3. El listado devuelve solo los **activos**.

---

## Historia 7 — Registrar y corregir líneas de investigación

| | | |
|---|---|---|
| **Usuario:** Mauricio Álvarez — jefe de talento docente | **Prioridad:** Media | **Riesgo:** Bajo |
| **Puntos:** 3 | **Horas:** 7 | **Versión:** v1 |

**Yo, Mauricio Álvarez, quiero registrar las líneas de investigación con
su nombre y descripción, para poder adscribir docentes a ellas más
adelante.**

**Criterios de aceptación:**

1. Nombre y descripción son obligatorios: sin alguno, **422**.
2. Una línea se registra **aunque todavía no tenga docentes adscritos**.
3. La corrección parcial permite cambiar solo la descripción y responde
   **200**.

---

## Historia 8 — Que un listado largo no tumbe la pantalla

| | | |
|---|---|---|
| **Usuario:** Claudia Mesa — analista de docencia | **Prioridad:** Media | **Riesgo:** Bajo |
| **Puntos:** 1 | **Horas:** 3 | **Versión:** v1 |

**Yo, Claudia Mesa, quiero pedir solo los primeros N registros, porque con
ciento noventa y un programas y doscientas dieciocho áreas no hay pantalla
que aguante.**

**Criterios de aceptación:**

1. Los cinco listados aceptan un parámetro de límite.
2. La respuesta dice **cuántos** registros trae y **cuál límite** se
   aplicó.
3. Si el límite no es un número, responde **422**.
4. Sin límite, se aplica uno por defecto y se informa cuál.

---

## Historia 9 — Que los datos no se corrompan

| | | |
|---|---|---|
| **Usuario:** Dra. Patricia Uribe — vicerrectora académica | **Prioridad:** Alta | **Riesgo:** Alto |
| **Puntos:** 3 | **Horas:** 8 | **Versión:** v1 |

**Yo, Patricia Uribe, necesito que el sistema no acepte datos que dejen la
base inconsistente, porque con esto se responde a la agencia
acreditadora.**

**Criterios de aceptación:**

1. Ningún dato del cliente se concatena a una consulta SQL: van
   **parametrizadas**.
2. Dos identificadores iguales en la misma tabla no se pueden guardar.
3. Un campo obligatorio vacío se rechaza con **422**, no se guarda como
   cadena vacía.
4. El error dice **qué campo** está mal, sin exponer el mensaje interno
   del motor de base de datos.

---

## Historia 10 — Ver y editar desde una pantalla

| | | |
|---|---|---|
| **Usuario:** Claudia Mesa — analista de docencia | **Prioridad:** Alta | **Riesgo:** Medio |
| **Puntos:** 5 | **Horas:** 14 | **Versión:** v1 |

**Yo, Claudia Mesa, quiero una pantalla donde ver y editar los cinco
catálogos, para no depender de que alguien me arme las peticiones a
mano.**

**Criterios de aceptación:**

1. Hay un listado por cada una de las cinco tablas, con sus columnas.
2. Hay formulario para crear y editar, y los errores del servidor se
   muestran junto al campo, en palabras entendibles.
3. La pantalla llama a la API **por HTTP**: no se conecta a la base.
4. **Con la API apagada, la pantalla abre igual** y avisa.
5. Corre en **su propio contenedor**, y sube con el mismo comando.

---

## Resumen de la iteración

| # | Historia | Puntos | Horas |
|---|---|---|---|
| 1 | Registrar los programas académicos | 5 | 12 |
| 2 | Corregir un programa | 3 | 8 |
| 3 | Retirar un programa que se cerró | 3 | 6 |
| 4 | Registrar las redes académicas | 2 | 5 |
| 5 | Mantener el catálogo de áreas de conocimiento | 3 | 8 |
| 6 | Registrar términos clave con su traducción | 3 | 7 |
| 7 | Registrar y corregir líneas de investigación | 3 | 7 |
| 8 | Que un listado largo no tumbe la pantalla | 1 | 3 |
| 9 | Que los datos no se corrompan | 3 | 8 |
| 10 | Ver y editar desde una pantalla | 5 | 14 |
| | **Total** | **31** | **78** |

---

## Trazabilidad

| Historia | Sale de |
|---|---|
| 1 | Ronda 3 y 4: «salen tres programas donde hay uno»; los campos obligatorios |
| 2 | Ronda 4: los graduados cambian cada semestre |
| 3 | Ronda 3: Mauricio retira; Patricia exige conservar graduados y experiencia |
| 4 | Ronda 2 y 4: Hernán y sus tres redes; la web opcional |
| 5 | Ronda 4: los tres niveles del OCDE |
| 6 | Ronda 4: el término y su traducción; no puede haber dos iguales |
| 7 | Ronda 4: nombre y descripción |
| 8 | Ronda 4: 191 programas y 218 áreas |
| 9 | Ronda 4: «que lo rechace y diga cuál campo» |
| 10 | Ronda 1 y 3: el PDF y el Excel que nunca dicen lo mismo |

**El «para qué» de todas** sale de la ronda 1: las dos semanas de correos
que hoy cuesta una acreditación, y los 191 programas de los que nadie sabe
quién dicta qué.
