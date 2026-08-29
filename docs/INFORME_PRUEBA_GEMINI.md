# Informe de la prueba — construir la v1 con IA

> **Qué se probó:** entregarle a una IA los 8 documentos del spec kit de la
> v1 y el prompt de la [GUIA_IA1](spec_kit/versiones/v1_area_conocimiento/GUIA_IA1.md),
> sin ayudarla por fuera, y ver si el sistema salía funcionando.
>
> **Herramienta:** Gemini (chat web) · **Fecha:** 29 de agosto de 2026 ·
> **Resultado:** la v1 funciona, con **los 7 criterios de aceptación en
> verde** y verificados contra el sistema corriendo.

---

## 1. Por qué existe este documento

Cada cambio que se le hace a un prompt o a una especificación tiene que
poder justificarse. Sin un registro, los prompts crecen por corazonada:
alguien tuvo un problema, agregó una regla, y tres meses después nadie sabe
por qué está ahí ni si sigue haciendo falta.

Aquí queda **qué falló, de quién era la culpa y cómo se corrigió** — con la
clasificación de los tres destinos de la
[GUIA_IA1](spec_kit/versiones/v1_area_conocimiento/GUIA_IA1.md): la
corrección va a la **spec**, al **prompt**, o la hace el **estudiante**.

## 2. El resultado, en una tabla

| | |
|---|---|
| Archivos que produjo la IA | 16 de la API, más el `docker-compose.yml` |
| Criterios de aceptación en verde | **7 de 7**, corridos contra el sistema |
| Hallazgos | **12** |
| De ellos, culpa del prompt | **2** — y las dos eran regresiones nuestras |
| De ellos, huecos de la especificación | **9** |
| De ellos, errores propios de la IA | **1** |

**El dato que más importa:** de doce hallazgos, **once eran defectos de los
documentos**. La IA hizo casi siempre lo que le pedimos; el problema era lo
que le pedimos.

## 3. Los hallazgos, uno por uno

### Van al PROMPT (2)

| # | Qué pasó | Causa | Corrección |
|---|---|---|---|
| 1 | **No entregó el `docker-compose.yml`.** Dio por hecha toda la Fase 0 y pidió ejecutar un comando que no podía funcionar, porque ese archivo estaba vacío | Al adaptar el prompt del curso se perdieron seis palabras: *"se montan tal cual **en el compose**"*. Eran las que decían que el compose todavía había que escribirlo. Y se agregó *"la tabla existe, tiene 218 filas"*, que refuerza lo contrario | Se recupera la frase del molde y se agrega, explícito: *"el `docker-compose.yml` TODAVÍA NO EXISTE y escribirlo es tu primera tarea"* |
| 2 | La IA no sabía qué hacer cuando se le pega un error | Al reordenar las reglas se **eliminó una entera**: *"los errores NO nos frenan"*, que además le dice que al final guíe el smoke test | Vuelve como regla 3. El prompt pasa de 7 a 8 reglas, las mismas del molde |

> **Las dos son regresiones introducidas al adaptar la plantilla**, no
> defectos de la IA ni del método. En el ejemplo del curso esto funcionaba.
> La lección es incómoda y útil: **adaptar un prompt es más delicado que
> escribirlo**, porque lo que se borra no se ve.

### Van a la ESPECIFICACIÓN (9)

**En `8_tasks.md` (4):**

| # | Qué pasó | Corrección |
|---|---|---|
| 3 | La Fase 0 se titulaba *"(artefacto dado)"* y mezclaba lo que ya viene con lo que hay que construir | Se separan las dos listas |
| 4 | La verificación decía *"un `SELECT COUNT(*)` que responda **218 · 17 · 21 · 6**"* **sin nombrar las tablas**. La IA rellenó el hueco: repartió los números al azar y **se inventó una tabla, `ac_sublinea`, que no existe en ninguna parte** | La verificación nombra las cuatro tablas |
| 5 | Las fases 1 a 4 verificaban con `dotnet build`, y **ninguna podía compilar**: sin `Program.cs` no hay punto de entrada, y llegaba en la fase 5. Toda la cadena de compuertas era impasable | El `Program.cs` mínimo se adelanta a la Fase 1, que pasa a verificarse con la API respondiendo |
| 6 | `appsettings.json` estaba en el plan y **ninguna tarea lo construía** | Se agrega a la Fase 1 |

**En `3_plan.md` (4):**

| # | Qué pasó | Corrección |
|---|---|---|
| 7 | El compose salió con `depends_on` a secas, **sin `healthcheck`**. SQL Server tarda entre 30 y 60 segundos en aceptar conexiones: el inicializador habría corrido contra un motor mudo y la API habría quedado hablándole a una base vacía | §5 pasa a decir el **CÓMO**: el healthcheck y las dos condiciones, más los puertos, los volúmenes y el nombre de la cadena |
| 8 | El servicio recibía las clases de `Peticiones/`: la **capa 2 dependía de la forma del cuerpo HTTP** | §4.7 nuevo, con la regla para revisarlo de un vistazo: *un `using` de `Peticiones` en `Servicios/` o `Repositorios/` significa capa rota* |
| 9 | **El 422 no salía.** Con `[ApiController]`, un cuerpo inválido responde 400 con `ProblemDetails` **antes de entrar al método** — no hay `catch` donde arreglarlo. De ese 422 dependen dos criterios | §4.9 nuevo, con la fábrica de respuestas que hay que reemplazar |
| 10 | El `.csproj` no excluía `pruebas/`, así que **el repositorio de mentiras se compilaba dentro de la API**. El síntoma es una advertencia, no un error: pasa desapercibida | Se documenta la exclusión en §2 |

**En `6_contracts.md` (1):**

| # | Qué pasó | Corrección |
|---|---|---|
| 11 | **El contrato se contradecía a sí mismo:** `gran_area` en los cuerpos y `filasAfectadas` en las respuestas. Dos convenciones de nombres imposibles de cumplir a la vez, porque la configuración del serializador es global | Se unifica en **camelCase**, justificado en `D-v1-8`: es el comportamiento por defecto de ASP.NET, y lo que no se configura no se puede configurar mal |

> Este es el único hallazgo que **ninguna revisión de documentos encontró**.
> Lo encontró el código: solo al escribir la primera clase de petición se
> vio que las dos convenciones no podían coexistir.

### Lo corrige el ESTUDIANTE (1)

| # | Qué pasó | Corrección |
|---|---|---|
| 12 | La ruta se declaró como `[Route("api/[controller]")]`, que genera `api/AreaConocimiento` — **sin guion bajo**. La URL del contrato no habría respondido | Se escribe la ruta completa. El contrato la decía con todas las letras: la IA la ignoró |

## 4. Qué NO falló

Conviene decirlo, porque el informe podría leerse como una lista de
desastres:

- **Leyó los documentos y los citó como argumento.** Justificó la
  contraseña en el compose citando el Artículo 7, el contenedor
  inicializador citando la decisión D-v1-6, y la ausencia de la propiedad
  `Activo` en la entidad citando el modelo de datos.
- **No inventó versiones de paquetes.** Las tres restauraron a la primera.
- **Las cinco consultas del repositorio llevan `WHERE activo = 1`** y
  ninguna concatena valores. Las dos cosas que más se temían del borrado
  lógico y de la inyección SQL salieron bien a la primera.
- **Acertó la regla más sutil del servicio:** que un `PATCH` vacío debe
  lanzar `ArgumentException` y no dejar que el repositorio devuelva cero
  filas, porque cero significa "no existe" y respondería 404 en vez de 400.

## 5. Las tres lecciones

1. **Los huecos de una especificación no se ven leyéndola; se ven
   construyendo con ella.** El `9_checklist.md` encontró cuatro defectos
   antes de escribir código —y valió la pena—, pero los otros ocho solo
   aparecieron cuando alguien intentó ejecutar lo que decía.
2. **La especificación dice el QUÉ; el plan tiene que decir el CÓMO.**
   Siete de los doce hallazgos son la misma falla repetida: el documento
   enunciaba el propósito (*"espera a que el motor responda"*, *"la
   validación produce 422"*) y no el mecanismo. Un propósito sin mecanismo
   se completa a criterio de quien construya — y la IA completa con lo más
   frecuente, no con lo correcto.
3. **Cuando la IA no sabe, no pregunta: completa.** Le pedimos
   explícitamente que preguntara ante cualquier ambigüedad, y aun así se
   inventó una tabla antes que preguntar de qué tablas eran esos conteos.
   Esa es la razón de que las compuertas existan.

## 6. Lo que queda pendiente

- **Volver a correr la prueba desde cero**, con los documentos ya
  corregidos y una IA distinta, para ver cuántos de los doce hallazgos
  desaparecen. Es la única forma de saber si las correcciones sirvieron o
  solo nos hicieron sentir mejor.
- Adaptar los documentos conceptuales del curso (flujo de una petición,
  SOLID y capas, conceptos de Docker) a este repositorio.
