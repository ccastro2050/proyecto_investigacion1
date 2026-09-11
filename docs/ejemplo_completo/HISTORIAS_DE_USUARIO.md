# Historias de usuario — Recolección de residuos electrónicos (RAEE)

**Versión 1 · Institución Universitaria ITM**
**Tecnología en Desarrollo de Software — Aplicación y Servicios Web**

| | |
|---|---|
| **Proyecto** | Recolección de residuos electrónicos (RAEE) |
| **Versión del documento** | 1.0 |
| **Fecha** | 11 de septiembre de 2026 |
| **Alcance** | Las tres tablas sin clave foránea —`tipo_residuo`, `gestor`, `ciudadano`— **con su front** |

---

## Historial de revisiones

| Fecha | Versión | Descripción | Autor |
|---|---|---|---|
| 11-09-2026 | 1.0 | Primera versión — historias de la v1 | Carlos Arturo Castro Castro |

---

## Introducción

Una historia de usuario describe algo que alguien necesita del sistema,
desde su punto de vista: *como [rol], quiero [acción], para [beneficio]*.

Las que siguen son **las de la versión 1**. No hay que redactar más: se
implementan estas, y **sus criterios de aceptación son los que se van a
verificar**.

**Los datos de personas y empresas de los ejemplos son inventados.** Los
correos usan `example.com`, reservado para documentación, y los teléfonos
usan el 555.

---

## Historia de usuario 1

| | | |
|---|---|---|
| **Número:** 1 | **Usuario:** Marcela Ruiz — coordinadora ambiental | |
| **Nombre historia:** registrar los tipos de residuo | | |
| **Diseñada por:** Carlos Arturo Castro Castro | | |
| **Prioridad:** Alta | **Riesgo en desarrollo:** Bajo | |
| **Puntos estimados:** 2 | **Horas estimadas:** 6 | **Iteración asignada:** v1 |

**Descripción:**

Yo, Marcela Ruiz, como coordinadora ambiental, quiero registrar los tipos
de residuo que se reciben —baterías de litio, pantallas, celulares,
cables— con su **peligrosidad** y su **valor por kilo**, para que cada
contenedor sepa qué recibe y para poder calcular cuánto se le reconoce al
ciudadano por lo que entrega.

**Criterios de aceptación:**

1. El listado devuelve los tipos **activos**, con identificador, nombre,
   peligrosidad y valor por kilo.
2. Si no hay ningún tipo activo, el sistema responde **204**, no una lista
   vacía.
3. Consultar un tipo que no existe, o que está inactivo, responde **404**.
4. Crear un tipo **sin nombre** o **sin valor por kilo** responde **422**,
   diciendo cuál campo falta.
5. La peligrosidad solo acepta `alta`, `media` o `baja`.
6. El valor por kilo se guarda con dos decimales.

**Observaciones:**
El valor por kilo es del tipo de residuo, no de cada entrega.

---

## Historia de usuario 2

| | | |
|---|---|---|
| **Número:** 2 | **Usuario:** Marcela Ruiz — coordinadora ambiental | |
| **Nombre historia:** corregir un tipo de residuo | | |
| **Diseñada por:** Carlos Arturo Castro Castro | | |
| **Prioridad:** Alta | **Riesgo en desarrollo:** Medio | |
| **Puntos estimados:** 3 | **Horas estimadas:** 8 | **Iteración asignada:** v1 |

**Descripción:**

Yo, Marcela Ruiz, quiero corregir un tipo ya registrado. Cuando cambia el
precio de la chatarra electrónica necesito actualizar **solo el valor por
kilo**, sin volver a escribir el nombre y la peligrosidad. Otras veces
reemplazo la ficha completa.

**Criterios de aceptación:**

1. El **reemplazo completo** exige todos los campos: si falta uno,
   responde **422**.
2. La **corrección parcial** acepta el mismo cuerpo incompleto y responde
   **200**, cambiando solo los campos que llegaron.
3. Los campos que no llegaron **conservan su valor anterior**.
4. Corregir un tipo que no existe responde **404** en los dos casos.

**Observaciones:**
Este es el criterio que separa reemplazar de actualizar: el mismo cuerpo
incompleto da **422** por el uno y **200** por el otro.

---

## Historia de usuario 3

| | | |
|---|---|---|
| **Número:** 3 | **Usuario:** Julián Restrepo — jefe de operaciones | |
| **Nombre historia:** registrar los gestores autorizados | | |
| **Diseñada por:** Carlos Arturo Castro Castro | | |
| **Prioridad:** Alta | **Riesgo en desarrollo:** Bajo | |
| **Puntos estimados:** 2 | **Horas estimadas:** 6 | **Iteración asignada:** v1 |

**Descripción:**

Yo, Julián Restrepo, quiero registrar las empresas gestoras autorizadas
—su nombre, su NIT, el número de la **resolución ambiental** que las
habilita y sus datos de contacto—, para saber a quién llamar cuando un
punto de acopio se llena y para demostrar ante la autoridad que quien
recogió el material estaba autorizado.

Un gestor se registra **aunque todavía no tenga puntos de acopio**:
primero se firma el convenio, después se abren los puntos.

**Criterios de aceptación:**

1. El listado devuelve los gestores **activos** con nombre, NIT,
   resolución, correo y teléfono.
2. Crear un gestor **sin resolución** responde **422**.
3. El teléfono se guarda como texto: acepta `+57 604 555 0401` completo.
4. Se puede crear un gestor sin que exista ningún punto de acopio.

---

## Historia de usuario 4

| | | |
|---|---|---|
| **Número:** 4 | **Usuario:** Julián Restrepo — jefe de operaciones | |
| **Nombre historia:** sacar un gestor del sistema | | |
| **Diseñada por:** Carlos Arturo Castro Castro | | |
| **Prioridad:** Media | **Riesgo en desarrollo:** Medio | |
| **Puntos estimados:** 2 | **Horas estimadas:** 5 | **Iteración asignada:** v1 |

**Descripción:**

Yo, Julián Restrepo, quiero **eliminar del sistema** un gestor cuando se
le vence la resolución ambiental, para que deje de aparecer en los
listados y nadie le asigne puntos nuevos.

Pero las entregas que ese gestor recibió el año pasado **tienen que seguir
apareciendo en los informes a la autoridad ambiental**: esos kilos ya se
reportaron y no se pueden perder.

**Criterios de aceptación:**

1. Al eliminar un gestor, deja de aparecer en el listado.
2. Consultarlo directamente después de eliminado responde **404**.
3. Eliminar dos veces el mismo gestor responde **404** la segunda vez.
4. **Los datos históricos de ese gestor se conservan** para los informes.

---

## Historia de usuario 5

| | | |
|---|---|---|
| **Número:** 5 | **Usuario:** Andrés Salazar — ciudadano | |
| **Nombre historia:** registrarme para entregar mis residuos | | |
| **Diseñada por:** Carlos Arturo Castro Castro | | |
| **Prioridad:** Alta | **Riesgo en desarrollo:** Bajo | |
| **Puntos estimados:** 2 | **Horas estimadas:** 6 | **Iteración asignada:** v1 |

**Descripción:**

Yo, Andrés Salazar, quiero registrarme con mis nombres, apellidos,
documento y datos de contacto, para que las entregas que haga queden a mi
nombre y me llegue la constancia.

**Criterios de aceptación:**

1. Crear un ciudadano **sin documento** responde **422**.
2. El listado devuelve solo los ciudadanos **activos**.
3. Consultar un ciudadano retirado responde **404**.
4. Un ciudadano se registra **aunque todavía no haya entregado nada**.

---

## Historia de usuario 6

| | | |
|---|---|---|
| **Número:** 6 | **Usuario:** Marcela Ruiz — coordinadora ambiental | |
| **Nombre historia:** que un listado largo no tumbe la pantalla | | |
| **Diseñada por:** Carlos Arturo Castro Castro | | |
| **Prioridad:** Media | **Riesgo en desarrollo:** Bajo | |
| **Puntos estimados:** 1 | **Horas estimadas:** 3 | **Iteración asignada:** v1 |

**Descripción:**

Yo, Marcela Ruiz, quiero pedir **solo los primeros N registros** de
cualquier listado, porque cuando entren los ciudadanos de toda la ciudad
no quiero esperar a que lleguen diez mil filas para ver diez.

**Criterios de aceptación:**

1. Los tres listados aceptan un parámetro de límite.
2. La respuesta dice **cuántos** registros trae y **cuál fue el límite**.
3. Si el límite no es un número, responde **422**.
4. Si no llega ningún límite, se aplica uno por defecto y se informa cuál.

---

## Historia de usuario 7

| | | |
|---|---|---|
| **Número:** 7 | **Usuario:** Diana Gómez — auditora ambiental | |
| **Nombre historia:** que los datos no se corrompan | | |
| **Diseñada por:** Carlos Arturo Castro Castro | | |
| **Prioridad:** Alta | **Riesgo en desarrollo:** Alto | |
| **Puntos estimados:** 3 | **Horas estimadas:** 8 | **Iteración asignada:** v1 |

**Descripción:**

Yo, Diana Gómez, como auditora, necesito que el sistema **no acepte datos
que dejen la base inconsistente**, aunque quien los mande sea un programa
y no una persona: lo que aquí se guarda se reporta a la autoridad.

**Criterios de aceptación:**

1. Ningún dato del cliente se pega dentro de una consulta SQL: las
   consultas van **parametrizadas**.
2. Dos identificadores iguales en la misma tabla no se pueden guardar.
3. Un campo obligatorio vacío se rechaza con **422**, no se guarda como
   cadena vacía.
4. La respuesta de error dice **qué campo** está mal, sin exponer el
   mensaje interno del motor de base de datos.

---

## Historia de usuario 8

| | | |
|---|---|---|
| **Número:** 8 | **Usuario:** Marcela Ruiz — coordinadora ambiental | |
| **Nombre historia:** ver y editar todo desde una pantalla | | |
| **Diseñada por:** Carlos Arturo Castro Castro | | |
| **Prioridad:** Alta | **Riesgo en desarrollo:** Medio | |
| **Puntos estimados:** 5 | **Horas estimadas:** 14 | **Iteración asignada:** v1 |

**Descripción:**

Yo, Marcela Ruiz, quiero **una pantalla** donde ver los tipos de residuo,
los gestores y los ciudadanos, y poder crear y corregir cada uno, para no
depender de que alguien me arme las peticiones a mano.

No me importa con qué esté hecha. Me importa que abra en el navegador, que
muestre los datos que hay, y que cuando el servidor esté caído **me lo
diga en español** en vez de mostrarme una página en blanco.

**Criterios de aceptación:**

1. Hay un listado por cada una de las tres tablas, con sus columnas.
2. Hay formulario para **crear** y para **editar**, y los errores del
   servidor se muestran junto al campo, en palabras entendibles.
3. La pantalla obtiene los datos **llamando a la API por HTTP**. No se
   conecta a la base: no tiene el driver ni las credenciales.
4. **Con la API apagada, la pantalla abre igual** y avisa que el servicio
   no está disponible.
5. La pantalla corre en **su propio contenedor** y queda arriba con el
   mismo `docker compose up -d --build`.

**Observaciones:**
La tecnología la elige quien construye. Lo que no cambia es la
separación: pantalla → API → base de datos, nunca pantalla → base.

---

## Resumen de la iteración

| # | Historia | Puntos | Horas |
|---|---|---|---|
| 1 | Registrar los tipos de residuo | 2 | 6 |
| 2 | Corregir un tipo de residuo | 3 | 8 |
| 3 | Registrar los gestores autorizados | 2 | 6 |
| 4 | Sacar un gestor del sistema | 2 | 5 |
| 5 | Registrarme para entregar mis residuos | 2 | 6 |
| 6 | Que un listado largo no tumbe la pantalla | 1 | 3 |
| 7 | Que los datos no se corrompan | 3 | 8 |
| 8 | Ver y editar todo desde una pantalla | 5 | 14 |
| | **Total** | **20** | **56** |

---

## Referencias

- Enunciado: «Evaluación individual — Aplicación y Servicios Web».
- `SDD_SPECKIT.md`, `SOLID_CAPAS_PATRONES.md` y `PARADIGMA_POO.md`, en la
  carpeta `docs/` de cualquiera de las plantillas del curso.
