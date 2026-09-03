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

---

## D-v1-9 — El front es un TERCER PROCESO, y no comparte código con la API

**Lo que se decidió.** El front va en su propio contenedor, en su propio
puerto, con su propio proyecto de .NET. Habla con la API **solo por HTTP**.

**Lo que se descartó.**

| Alternativa | Por qué no |
|---|---|
| **Servir las páginas desde la misma API** (Razor Pages en el proyecto de la API) | Un solo proceso: la separación entre presentación y datos pasaría a ser una convención que nadie puede verificar. Y apagar «la API» apagaría también la pantalla, así que la prueba del criterio 11 dejaría de existir |
| **Compartir la clase `AreaConocimiento`** con una referencia de proyecto | Ataría los dos procesos: un cambio interno de la API —renombrar una propiedad— rompería el front **sin que nadie tocara el contrato**. Lo único que deben compartir es el JSON |
| **Un `ApiService` genérico** con el nombre de la tabla como parámetro | Sección 6.1 de la metodología: un método `Listar(string tabla)` no dice qué recursos existen, y el compilador deja de revisar |

**Cómo se verifica que la decisión se cumple**, que es lo que la vuelve algo
más que una intención:

1. `FrontInvestigacion.csproj` **no tiene ningún paquete** de acceso a datos.
2. El servicio `front-blazor` **no depende de `sqlserver`** en el compose.
3. Y la prueba: `docker compose stop api-investigacion` deja la pantalla en
   pie, con su aviso y **sin un solo dato** (criterio 11).

> **Los dos en C# es lo que hace difícil esta decisión, no lo que la hace
> fácil.** Con el front en otro lenguaje, compartir código sería imposible y no
> habría nada que cuidar. Aquí la tentación existe todos los días, y por eso
> queda escrita.

---

## D-v1-10 — La pantalla no le habla al usuario en jerga

**Lo que se decidió.** En la pantalla no aparece ningún verbo HTTP, ningún
código de estado, ni el nombre de ninguna tabla. Los dos botones de guardar se
llaman **«Guardar la ficha completa»** y **«Guardar solo lo que cambié»**.

**Lo que se descartó:** nombrarlos «PUT» y «PATCH», que es lo que sale solo
cuando quien escribe la pantalla viene de escribir el controlador.

**Por qué importa más de lo que parece.** Quien usa esto administra un catálogo
de áreas de conocimiento. «PUT» no le dice nada, y peor: le sugiere que
necesita saber algo que no necesita. La distinción que sí le sirve —«¿mando
todo o solo lo que toqué?»— es exactamente la que los dos nombres explican.

Y no se pierde nada del contenido técnico: **el mismo formulario a medio llenar
que «la ficha completa» rechaza, «solo lo que cambié» lo guarda.** La lección
del contrato sigue ahí, y ahora se puede ver con los ojos.

> Se comprueba automáticamente, y **sobre el texto visible**, no sobre el HTML:
> el guion quita las etiquetas y decodifica las entidades antes de buscar. La
> primera versión buscaba en el código fuente y dio dos falsos positivos.
