# Decisiones — Versión 1

> Cada decisión con sus alternativas y su razón. Esto es memoria del
> proyecto: sirve para **no volver a discutir** lo ya discutido, y para que
> quien llegue después —persona o IA— entienda por qué el sistema es así.
>
> Numeración `D-v1-N`, sin repetir entre versiones.

## D-v1-1 — Dapper, no Entity Framework

**Contexto.** Hay que llevar filas de SQL Server a objetos de C#.

**Alternativas.** (a) **Entity Framework Core**: escribe el SQL por
nosotros, migraciones incluidas. (b) **Dapper**: mapea fila→objeto pero el
SQL lo escribimos nosotros. (c) **ADO.NET puro**: hasta el mapeo a mano.

**Decisión: (b).** El Artículo 2 exige que el SQL esté a la vista. Con EF
el SQL que llega al motor lo genera un traductor: se pierde justo lo que
este curso quiere que se vea y se sustente en una revisión. ADO.NET puro
agregaría veinte líneas de `reader.GetString(i)` por consulta sin enseñar
nada nuevo.

**Consecuencias.** No hay migraciones: el esquema viene dado (Artículo 5).
Cada consulta hay que escribirla, y por eso mismo se puede leer.
**Estado:** vigente.

## D-v1-2 — Las tres capas desde el día 1, no un MVP en un archivo

**Contexto.** Una tabla y siete endpoints caben en un solo controlador de
80 líneas.

**Alternativas.** (a) Todo en el controlador y refactorizar en la v2.
(b) Capas con interfaces desde el principio.

**Decisión: (b).** "Refactorizar después" es una promesa que nadie cumple
con fecha de entrega encima, y la v2 llega con diez tablas: el momento de
separar sería el peor posible. Además, la prueba sin base de datos
—criterio 7— es **imposible** sin la interfaz del repositorio.

**Consecuencias.** Seis archivos donde cabría uno. A cambio, la v2 agrega
tablas sin tocar la arquitectura. **Estado:** vigente.

## D-v1-3 — Una petición por verbo

**Contexto.** `POST`, `PUT` y `PATCH` reciben cuerpos parecidos pero con
reglas distintas: el `PATCH` admite campos ausentes y el `PUT` no.

**Alternativas.** (a) Una sola clase con todos los campos opcionales y
validar a mano según el verbo. (b) Tres clases: `Crear`, `Reemplazo`,
`Actualizar`.

**Decisión: (b).** Con (a) la regla queda escondida en `if`s dentro del
servicio; con (b) la declara el tipo, y el 422 lo produce el framework
antes de que el negocio se entere. Es lo que hace demostrable la pareja
`PUT` 422 / `PATCH` 200 del criterio 4.

**Consecuencias.** Tres archivos pequeños y muy parecidos. Se acepta.
**Estado:** vigente.

## D-v1-4 — El borrado lógico se resuelve en el `UPDATE`, no consultando antes

**Contexto.** `DELETE` debe responder 404 si el registro no existe **o ya
está inactivo** (C4, C5).

**Alternativas.** (a) Consultar primero y luego actualizar. (b) Un solo
`UPDATE … WHERE id = @id AND activo = 1` y mirar las filas afectadas.

**Decisión: (b).** Una sola ida a la base, sin ventana entre la consulta y
la escritura. Cero filas significa exactamente "no existe o ya estaba
inactiva", que es la respuesta que pide la spec.

**Consecuencias.** El mensaje del 404 no distingue entre "nunca existió" y
"ya estaba borrada" — y **está bien**: para la API son el mismo caso.
**Estado:** vigente.

## D-v1-5 — El `id` es texto, y no se valida su formato

**Contexto.** Los códigos del catálogo (`1A01`) codifican la jerarquía
gran área → área → disciplina.

**Alternativas.** (a) Guardar como texto y aceptar cualquier cosa de hasta
6 caracteres. (b) Validar el patrón con una expresión regular.

**Decisión: (a).** Validar el patrón es una **regla de negocio** que
ninguna versión ha pedido, y que además haría difícil crear los códigos de
prueba del smoke test. Queda anotado por si una versión futura la pide.

**Consecuencias.** Se puede crear un `9Z01` que no sigue la jerarquía real
—y de hecho el smoke test lo usa a propósito—. **Estado:** vigente.

## D-v1-6 — Un contenedor aparte para inicializar la base

**Contexto.** SQL Server **no ejecuta los scripts que se le monten**:
alguien tiene que conectarse al motor y correrlos.

**Alternativas.** (a) Instrucciones manuales en el README ("conéctese y
corra esto"). (b) Un contenedor `sqlserver-init` que lo haga solo.

**Decisión: (b).** El Artículo 4 exige un solo comando; (a) lo rompe en la
primera línea. El inicializador espera a que el motor **responda
consultas**, crea la base si no existe, corre el script y se muere.

**Consecuencias.** Un servicio más en el compose, que termina en segundos y
es idempotente. **Estado:** vigente.

## D-v1-8 — Los campos JSON en camelCase

**Contexto.** Las columnas de la base son `gran_area`, `area`,
`disciplina`. ¿El JSON las repite tal cual, o usa la convención de C#?

**Alternativas.** (a) **snake_case** en el JSON, igual que la base: un
`SELECT` y una respuesta se leen igual. (b) **camelCase**, que es lo que
ASP.NET Core hace por defecto.

**Decisión: (b).** Tres razones, en orden de peso:

1. **Es el comportamiento por defecto**: cero configuración. Lo que no se
   configura no se puede configurar mal, y una política de serialización
   mal puesta rompe TODOS los endpoints a la vez, con un síntoma
   desconcertante — un `POST` correcto respondiendo "falta el campo".
2. **El JSON no es una ventana a la tabla.** La API es una frontera: si
   mañana la columna se renombra, el contrato no tiene por qué cambiar. El
   parecido con la base es una coincidencia cómoda, no un requisito.
3. **El front de la v4 lo consume directo.** `granArea` es lo que espera
   quien escribe JavaScript.

**Consecuencias.** El JSON deja de parecerse a la tabla, y hay que
traducir mentalmente al leer el repositorio. A cambio, `Program.cs` no
lleva ni una línea de configuración de serialización.

**De dónde salió esta decisión.** El `6_contracts.md` tenía las dos
convenciones mezcladas —`gran_area` en los cuerpos y `filasAfectadas` en
las respuestas de escritura—, y eso solo se descubrió al escribir la
primera clase de petición: era imposible cumplir las dos a la vez.
**Estado:** vigente.

## D-v1-7 — El catálogo se corrige antes de sembrarlo

**Contexto.** El Excel trae `Cienias Naturales` (sin la `c`) en 48 filas.

**Alternativas.** (a) Cargarlo tal cual: el dato es dado. (b) Corregir la
digitación al generar las semillas y documentarlo.

**Decisión: (b).** Un error de digitación no es un dato: es ruido de la
fuente. Cargarlo lo dejaría a la vista en cada listado, en cada informe y
en el dashboard de la v4. La corrección queda anotada en la cabecera de
`db/investigacion.sql`, de modo que cualquiera puede ver qué se cambió.

**Consecuencias.** El script deja de ser una copia literal del Excel, y por
eso mismo la cabecera del script tiene que decirlo. **Estado:** vigente.
