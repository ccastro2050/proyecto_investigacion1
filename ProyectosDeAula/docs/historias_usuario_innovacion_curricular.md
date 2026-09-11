# Historias de usuario — Módulo de Innovación Curricular

**Versión 1 · Las siete tablas sin claves foráneas**

| | |
|---|---|
| **Módulo** | Innovación Curricular — ver [`modulo_innovacion_curricular.md`](modulo_innovacion_curricular.md) |
| **De dónde salen** | [`entrevista_innovacion_curricular.md`](entrevista_innovacion_curricular.md) |
| **Alcance** | `area_conocimiento`, `universidad`, `aspecto_normativo`, `practica_estrategia`, `enfoque`, `car_innovacion`, `aliado` — **con su front** |

---

## Historia 1 — Registrar los aspectos normativos con su fuente

| | | |
|---|---|---|
| **Usuario:** Esteban Ruiz — analista de acreditación | **Prioridad:** Alta | **Riesgo:** Bajo |
| **Puntos:** 3 | **Horas:** 8 | **Versión:** v1 |

**Yo, Esteban Ruiz, quiero registrar las leyes y decretos que aplican, con
su fuente, para no perder media mañana buscando la norma que sé que
existe.**

**Criterios de aceptación:**

1. Tipo, descripción y **fuente** son obligatorios: si falta alguno,
   responde **422** e indica cuál.
2. El tipo solo acepta `ley`, `decreto`, `resolucion` o `acuerdo`.
3. El listado devuelve los **activos** y dice cuántos hay.
4. Consultar uno inexistente o inactivo responde **404**.

> *De la ronda 4: «sin fuente no sirve: en acreditación hay que citar de
> dónde salió», y el ejemplo del Decreto 1330 de 2019.*

---

## Historia 2 — Separar enfoques de prácticas

| | | |
|---|---|---|
| **Usuario:** Gloria Naranjo — directora de currículo | **Prioridad:** Alta | **Riesgo:** Medio |
| **Puntos:** 5 | **Horas:** 12 | **Versión:** v1 |

**Yo, Gloria Naranjo, quiero que los enfoques y las prácticas se registren
en listas separadas, para que nadie meta «aula invertida» entre los
enfoques y las consultas dejen de tener sentido.**

**Criterios de aceptación:**

1. Son **dos recursos distintos**: `/api/enfoques` y
   `/api/practicas-estrategias`.
2. Crear un enfoque **sin descripción** responde **422**.
3. En prácticas y estrategias, el **tipo** es obligatorio y separa una de
   otra.
4. Cada listado devuelve solo los **activos** y dice cuántos hay.

---

## Historia 3 — Registrar las características de innovación

| | | |
|---|---|---|
| **Usuario:** Gloria Naranjo — directora de currículo | **Prioridad:** Media | **Riesgo:** Bajo |
| **Puntos:** 2 | **Horas:** 5 | **Versión:** v1 |

**Yo, Gloria Naranjo, quiero registrar las características de innovación
—flexibilidad, interdisciplinariedad, internacionalización— para poder
decir qué persigue cada propuesta y no solo qué hace.**

**Criterios de aceptación:**

1. Nombre, descripción y tipo son obligatorios: sin alguno, **422**.
2. El listado devuelve solo las **activas**.
3. La corrección parcial permite cambiar solo la descripción y responde
   **200**.

---

## Historia 4 — Retirar una estrategia abandonada

| | | |
|---|---|---|
| **Usuario:** Gloria Naranjo — directora de currículo | **Prioridad:** Media | **Riesgo:** Alto |
| **Puntos:** 3 | **Horas:** 6 | **Versión:** v1 |

**Yo, Gloria Naranjo, quiero retirar del listado una estrategia que se
abandonó porque no funcionó, para que ningún programa la escoja de
nuevo.**

**Criterios de aceptación:**

1. Al retirarla, deja de aparecer en el listado.
2. Consultarla directamente después responde **404**.
3. Retirarla dos veces responde **404** la segunda vez.
4. **Los programas que la reportaron en su renovación siguen pudiendo
   citarla**: el documento de acreditación no puede quedar apuntando a
   algo que el sistema dice que no existe.

> *Los criterios 1 a 3 son de Gloria; el 4 es la objeción de Esteban.*

---

## Historia 5 — Registrar los aliados por su NIT

| | | |
|---|---|---|
| **Usuario:** Prof. Iván Betancur — coordinador de programa | **Prioridad:** Alta | **Riesgo:** Medio |
| **Puntos:** 3 | **Horas:** 7 | **Versión:** v1 |

**Yo, Iván Betancur, quiero registrar las entidades aliadas con su NIT,
razón social, contacto y ciudad, para que después cuadre con la oficina
jurídica.**

**Criterios de aceptación:**

1. NIT, razón social y ciudad son obligatorios: sin alguno, **422**.
2. **El NIT no puede repetirse**: identifica a la entidad, y dos veces el
   mismo NIT es la misma entidad dos veces.
3. El listado devuelve solo los aliados **activos**.
4. Un aliado se registra **aunque todavía no tenga convenios**.

---

## Historia 6 — Mantener el catálogo de áreas de conocimiento

| | | |
|---|---|---|
| **Usuario:** Esteban Ruiz — analista de acreditación | **Prioridad:** Alta | **Riesgo:** Bajo |
| **Puntos:** 3 | **Horas:** 8 | **Versión:** v1 |

**Yo, Esteban Ruiz, quiero mantener las áreas de conocimiento con sus tres
niveles, para clasificar cada programa con la misma lista que usa el resto
de la universidad.**

**Criterios de aceptación:**

1. Gran área, área y disciplina son obligatorias: sin alguna, **422**
   indicando cuál.
2. El listado devuelve las **activas** y dice cuántas hay.
3. Si no hay ninguna activa, responde **204**, no una lista vacía.

---

## Historia 7 — Mantener las sedes

| | | |
|---|---|---|
| **Usuario:** Esteban Ruiz — analista de acreditación | **Prioridad:** Media | **Riesgo:** Bajo |
| **Puntos:** 2 | **Horas:** 5 | **Versión:** v1 |

**Yo, Esteban Ruiz, quiero registrar las sedes y seccionales con su
ciudad, para poder decir dónde se ofrece cada programa.**

**Criterios de aceptación:**

1. Nombre, tipo —sede o seccional— y ciudad son obligatorios.
2. El tipo solo acepta esos dos valores.
3. Corregir el nombre **no afecta** lo que cuelga de la sede.

---

## Historia 8 — Que un listado largo no tumbe la pantalla

| | | |
|---|---|---|
| **Usuario:** Esteban Ruiz — analista de acreditación | **Prioridad:** Media | **Riesgo:** Bajo |
| **Puntos:** 1 | **Horas:** 3 | **Versión:** v1 |

**Yo, Esteban Ruiz, quiero pedir solo los primeros N registros de
cualquier listado, para no cargar doscientas dieciocho áreas cuando
necesito diez.**

**Criterios de aceptación:**

1. Los siete listados aceptan un parámetro de límite.
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
base inconsistente, porque de aquí salen los documentos de acreditación.**

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
| **Usuario:** Gloria Naranjo — directora de currículo | **Prioridad:** Alta | **Riesgo:** Medio |
| **Puntos:** 5 | **Horas:** 14 | **Versión:** v1 |

**Yo, Gloria Naranjo, quiero una pantalla donde ver y editar los siete
catálogos, para que un coordinador que va a proponer algo vea en dos
minutos qué se ha intentado antes.**

**Criterios de aceptación:**

1. Hay un listado por cada una de las siete tablas, con sus columnas.
2. Hay formulario para crear y editar, y los errores del servidor se
   muestran junto al campo, en palabras entendibles.
3. La pantalla llama a la API **por HTTP**: no se conecta a la base.
4. **Con la API apagada, la pantalla abre igual** y avisa.
5. Corre en **su propio contenedor** y sube con el mismo comando.

---

## Resumen de la iteración

| # | Historia | Puntos | Horas |
|---|---|---|---|
| 1 | Registrar los aspectos normativos con su fuente | 3 | 8 |
| 2 | Separar enfoques de prácticas | 5 | 12 |
| 3 | Registrar las características de innovación | 2 | 5 |
| 4 | Retirar una estrategia abandonada | 3 | 6 |
| 5 | Registrar los aliados por su NIT | 3 | 7 |
| 6 | Mantener el catálogo de áreas de conocimiento | 3 | 8 |
| 7 | Mantener las sedes | 2 | 5 |
| 8 | Que un listado largo no tumbe la pantalla | 1 | 3 |
| 9 | Que los datos no se corrompan | 3 | 8 |
| 10 | Ver y editar desde una pantalla | 5 | 14 |
| | **Total** | **30** | **76** |

---

## Trazabilidad

| Historia | Sale de |
|---|---|
| 1 | Ronda 3 y 4: «sé que existe un decreto, pero no dónde está»; la fuente |
| 2 | Ronda 3: «aula invertida no es un enfoque, es una práctica» |
| 3 | Ronda 3: la diferencia entre enfoque, práctica y característica |
| 4 | Ronda 3: Gloria retira; Esteban exige que la acreditación no quede coja |
| 5 | Ronda 2 y 4: «el NIT es la identificación» |
| 6 | Ronda 2: los catálogos que hacen falta antes que nada |
| 7 | Ronda 4: nombre, tipo y ciudad |
| 8 | Ronda 4: pedir un pedazo y saber cuántos hay |
| 9 | Ronda 4: «que lo rechace y diga cuál campo» |
| 10 | Ronda 1 y 4: los PDF de doscientas páginas que no se pueden consultar |

**El «para qué» de todas** sale de la ronda 1: tres programas montando lo
mismo sin saberlo, y un mes por programa para armar el anexo de
acreditación.
