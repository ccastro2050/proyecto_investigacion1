# Cómo construir la v1 con IA — por chat o con un IDE agéntico

> Los dos caminos para construir esta versión con ayuda de IA. La clave es
> la misma en ambos: **la IA no inventa, sigue el spec kit.** Usted
> verifica; la IA propone (chat) o ejecuta bajo su supervisión (IDE).
>
> Antes de abrir cualquiera de los dos, el [9_checklist.md](9_checklist.md)
> tiene que estar en verde. Construir sobre una spec sin revisar es
> multiplicar el error.

## 0. Los dos caminos

| | **Camino A: chat web** | **Camino B: IDE agéntico** |
|---|---|---|
| Herramientas | Gemini, DeepSeek, ChatGPT, Claude | Antigravity, Cursor, Claude Code, Copilot agente |
| ¿Cómo conoce la spec? | Usted le **sube los 8 archivos** | El agente **lee `docs/spec_kit/`** |
| ¿Quién escribe los archivos? | Usted pega lo que la IA propone | El agente los escribe |
| ¿Quién ejecuta? | Usted, y pega la salida | El agente, pidiendo permiso |
| Su papel | Operador: pegar, ejecutar y reportar | Supervisor: revisar diffs y aprobar |
| Riesgo típico | La IA pierde el contexto en chats largos | El agente avanza de más: hace tres fases de un tirón |

## Camino A — Chat web

### A.1 Qué subirle: 8 archivos

| # | Archivo |
|---|---|
| 1 | `docs/spec_kit/1_constitution.md` |
| 2 | `2_spec.md` |
| 3 | `3_plan.md` |
| 4 | `4_research.md` |
| 5 | `5_data_model.md` |
| 6 | `6_contracts.md` |
| 7 | `7_quickstart.md` |
| 8 | `8_tasks.md` |

**No suba nada más.** El `0_mapa_versiones.md` le revelaría lo que viene, y
la regla es que la v1 no anticipa.

> **¿Y el `9_checklist.md`?** Tampoco: no es material para la IA. Es la
> lista con la que **usted** revisó la spec antes de llegar aquí.
>
> **¿Y `db/investigacion.sql`?** Tampoco. Es un artefacto **dado**: la base
> ya existe con sus 218 filas. Si la IA intenta escribirle un
> `CREATE TABLE`, recuérdele el Artículo 5.

### A.2 El prompt (cópielo tal cual como PRIMER mensaje)

```text
Actúa como mi asistente de programación para construir la VERSIÓN 1 de un
proyecto universitario, partiendo de cero. Te adjunto 8 documentos: una
constitución (reglas permanentes) y el spec kit de la versión 1 (spec,
plan, research con las decisiones, modelo de datos, contratos, quickstart
y tareas).

El proyecto es C# sobre ASP.NET Core (.NET 10) + SQL Server, con Dapper y
el SQL escrito a mano — así lo fija 3_plan.md. Si en tu respuesta aparece
OTRO lenguaje o framework, significa que no leíste los adjuntos: detente y
dímelo en vez de continuar.

REGLAS DE TRABAJO (no negociables):

1. La especificación manda. No agregues NADA que los documentos no pidan:
   ni paquetes extra, ni Entity Framework, ni tablas de más, ni "mejoras"
   de tu cosecha. Si crees que falta algo, o si un documento admite dos
   lecturas, PREGÚNTAME antes: no lo resuelvas por tu cuenta ni "asumas"
   nada. Yo anotaré la respuesta en la sección de Clarificaciones de mi
   2_spec.md.
2. Vamos a seguir 8_tasks.md FASE POR FASE, en orden. En cada fase:
   a. Me explicas en 3-5 líneas qué vamos a hacer y por qué.
   b. Me entregas los archivos DE A UNO: primero la ruta exacta y el
      contenido COMPLETO de UN solo archivo, con los comentarios en
      español que exige la constitución: explican POR QUE está hecho así,
      no qué hace cada palabra del lenguaje. Esperas mi "listo"
      y solo entonces me das el siguiente.
   c. Al cerrar la fase me dices su comando de verificación y qué salida
      esperar.
   NOTA: la estructura de carpetas y los archivos vacíos YA EXISTEN en mi
   proyecto — no me des comandos para crearlos; tu trabajo es dictarme el
   CONTENIDO de cada archivo.
3. Los errores NO nos frenan. Si te pego un error, lo diagnosticas y me
   das el archivo completo corregido; si no sale rápido, seguimos con las
   fases siguientes y lo retomamos al final. Al terminar todas las fases
   me guías para correr el smoke test de 7_quickstart.md y corregimos
   juntos lo que salga.
4. La base de datos YA VIENE DADA en db/investigacion.sql + db/init.sh —
   se montan tal cual en el compose; no escribas ni modifiques SQL de
   creación de tablas. OJO: el docker-compose.yml TODAVÍA NO EXISTE y
   escribirlo es tu primera tarea (Fase 0). La tabla area_conocimiento ya
   tiene sus 218 filas y su llave primaria es un CÓDIGO DE TEXTO (por
   ejemplo '1A01'), no un entero.
5. El borrado es LÓGICO: DELETE marca activo = 0, y los listados filtran
   los inactivos. Nunca se borra la fila.
6. El código debe cumplir 6_contracts.md al pie de la letra: mismos
   verbos, mismas rutas, mismos códigos de estado y formatos de respuesta,
   incluido el contraste PUT (reemplazo completo → 422 si falta un campo)
   vs PATCH (parcial → 200 con el mismo cuerpo).
7. Todo en español: nombres, comentarios y mensajes.
8. Trabajo en Windows con VS Code (terminal integrada de PowerShell) y
   Docker Desktop. Dame los comandos para ese entorno. La API publica el
   puerto 8070, SQL Server el 11470 y EL FRONT el 8071.

9. LA VERSIÓN INCLUYE SU PANTALLA, y esto es la mitad del trabajo, no un
   añadido. Un FRONT en BLAZOR SERVER (.NET 10), en su propio proyecto y en
   su propio contenedor:

   · una pantalla por recurso, con DIRECCIÓN PROPIA (/areas-de-conocimiento),
     nunca una ruta con el nombre de la tabla como parámetro;
   · un SERVICIO POR RECURSO —ServicioAreaConocimiento con seis métodos—,
     nunca un ApiService genérico con la tabla como parámetro;
   · la pantalla NO le habla al usuario en jerga: ni PUT, ni PATCH, ni 422,
     ni el nombre de la tabla. Los dos botones de guardar se llaman
     "Guardar la ficha completa" y "Guardar solo lo que cambié";
   · y un error de la API NO borra lo que la persona había escrito.

   TRES COSAS QUE VAS A QUERER HACER Y NO DEBES:

   a) Compartir la clase AreaConocimiento entre la API y el front con una
      referencia de proyecto. NO: el front tiene la suya. Están los dos en C#
      y aun así lo único que comparten es el JSON. Si se compartiera, un
      cambio interno de la API rompería el front sin que nadie tocara el
      contrato.
   b) Servir las páginas desde la misma API. NO: son dos procesos, y eso hay
      que poder demostrarlo apagando uno.
   c) Meter Bootstrap o cualquier biblioteca por CDN. NO: el CSS va escrito a
      mano. Un front que necesita internet para verse bien no arranca en un
      salón sin red.

Al final, la versión 1 está TERMINADA solo cuando pasan los criterios de
aceptación de 2_spec.md —LOS ONCE, incluidos los cuatro de la pantalla—,
verificados con el smoke test de 7_quickstart.md.

Y hay un criterio que se comprueba apagando un contenedor: con la API
apagada, la pantalla tiene que SEGUIR RESPONDIENDO, con su menú y su aviso,
y SIN UN SOLO DATO. Si sigue mostrando el catálogo, el front está leyendo de
donde no debe.

Empieza: resume en máximo 10 líneas qué vamos a construir (para confirmar
que entendiste el alcance) y luego arranca con la Fase 0.
```

### A.3 El método de la conversación

1. **Pegue primero, ejecute cuando quiera.** Lo obligatorio es pegar cada
   archivo en su ruta y responder "listo". Las verificaciones de cada fase
   puede correrlas en el momento o dejarlas para el final.
2. **No se quede varado.** Si algo falla y no sale rápido, anótelo, siga
   con las fases siguientes y retómelo al final: muchos errores
   desaparecen cuando el sistema está completo.
3. **El punto de control real es el smoke test** de
   [7_quickstart.md](7_quickstart.md), corrido por usted.
4. **Si la primera respuesta llega en otro lenguaje**, la IA no leyó los
   adjuntos: cierre ese chat, verifique que los 8 cargaron y empiece de
   nuevo.
5. **Si la IA ASUME algo que la spec no dice, párela.** Lo va a mencionar
   de pasada —"asumo que el borrado es físico", "por defecto devuelvo
   409"— y ahí está el peligro, porque suena a detalle y es una
   **ambigüedad de la especificación**. Decida usted y **anote la
   respuesta en la sección 6 de [2_spec.md](2_spec.md)**, no solo en el
   chat. El chat se cierra; la spec queda.

## Camino B — IDE agéntico

### B.1 El prompt

```text
Construye la VERSIÓN 1 de este proyecto.

Primero lee, en este orden, los documentos que están bajo docs/spec_kit/
(1_constitution.md en la raíz; los demás en versiones/v1_area_conocimiento/):
1_constitution, 2_spec, 3_plan, 4_research, 5_data_model, 6_contracts,
7_quickstart y 8_tasks. Después resume en máximo 10 líneas qué vas a
construir y espera mi confirmación antes de tocar nada.

El código va en api_investigacion/ según la estructura de 3_plan.md.
docs/ y db/ son SOLO LECTURA: no los modifiques. La base de datos YA VIENE
DADA en db/investigacion.sql + db/init.sh — úsalos tal cual para montar
SQL Server. OJO: el docker-compose.yml todavía NO EXISTE y escribirlo es
tu primera tarea (Fase 0).

REGLAS (no negociables):

1. La especificación manda. No agregues nada que los documentos no pidan.
   Si crees que falta algo, o si un documento admite dos lecturas,
   PREGÚNTAME antes: no lo resuelvas por tu cuenta ni "asumas" nada. Yo
   anotaré la respuesta en la sección de Clarificaciones de 2_spec.md.
2. Sigue 8_tasks.md fase por fase. Al terminar cada fase EJECUTA su
   verificación, muéstrame la salida real, y espera mi OK antes de seguir.
3. El borrado es LÓGICO (activo = 0) y los listados filtran inactivos.
4. Cumple 6_contracts.md al pie de la letra, incluido PUT=422 vs
   PATCH=200 con el mismo cuerpo.
5. Todo en español, C# sobre ASP.NET Core (.NET 10) con Dapper y el SQL a
   la vista. API en el puerto 8070, SQL Server en el 11470.
6. Al final, corre el smoke test completo de 7_quickstart.md y muéstrame
   la evidencia de cada criterio de aceptación. La versión no está
   terminada hasta que todos estén en verde.
```

### B.2 Cómo supervisar

- **Revise cada diff** antes de aprobar: ¿el archivo está donde dice
  [3_plan.md](3_plan.md) §2? ¿Los comentarios explican por qué, no qué?
  ¿No agregó paquetes?
- **Freno de emergencia:** si hace varias fases de un tirón, deténgalo y
  pídale "vuelve a la fase N y muéstrame su verificación".
- **Cace las suposiciones:** cuando diga "asumo que…" o "por defecto voy
  a…", pare. Eso va a la sección 6 de `2_spec.md`.
- **No le crea "terminado":** pídale la salida real de los comandos. El
  criterio de cierre es el smoke test en verde.

## Cuando la IA se equivoca: los tres destinos

Se va a equivocar. Lo que decide si el trabajo mejora o se pudre es **a
dónde va cada corrección**, y hay tres destinos posibles:

| Si el error es… | La corrección va a… | Cómo se reconoce |
|---|---|---|
| **La IA no podía saberlo**: la spec no lo dice, o lo dice de dos maneras | **La spec**, como una Clarificación nueva | Usted mismo duda al contestarle. Si tiene que pensar la respuesta, no estaba escrita |
| **La spec lo dice, la IA lo ignoró — y vuelve a pasar** | **El prompt** | Se repite con otra IA, en otro chat, después de empezar de cero |
| **La spec lo dice claro y la IA falló una vez** | **Usted**: le señala el documento y sigue | Al corregirlo, no vuelve a ocurrir |

**La pregunta que separa el segundo del tercero es una sola: ¿se repite?**
Un error que aparece siempre viene del prompt —la regla existe pero no está
visible—. Uno que aparece una vez es ruido, y corregirlo es su trabajo de
supervisor: para eso está mirando.

> **Por qué importa no confundirlos.** Si por cada tropiezo se agrega una
> regla al prompt, el prompt termina con treinta reglas y **nadie lo lee**
> — ni la IA, que se pierde entre ellas, ni el siguiente estudiante. Un
> prompt que crece sin control es un prompt que dejó de funcionar.

**Y hay un cuarto camino que NUNCA se toma:** arreglar el código para que
"funcione" sin tocar la spec ni el prompt. Eso deja el documento diciendo
una cosa y el sistema haciendo otra — que es exactamente la deuda de
especificación contra la que existe este método.

## Lo que la IA rompe primero, en esta versión

Tres cosas que conviene vigilar desde el primer archivo:

| Qué | Por qué pasa |
|---|---|
| Olvidar `WHERE activo = 1` en algún listado | El borrado lógico no es lo más común en los ejemplos con los que se entrenó |
| Convertir el `id` en `int` | `id` suele ser entero; aquí es un código de texto (C1) |
| Devolver 409 en el código duplicado | Es lo "correcto" según el manual de HTTP, pero esta versión decidió 500 (C8) |

Las tres están decididas en las Clarificaciones. Si la IA propone otra
cosa, no está mejorando: está ignorando la spec.

### Y tres más, ahora que la versión tiene pantalla

| Qué | Por qué pasa | Qué revisar |
|---|---|---|
| **Compartir la clase entre la API y el front** | Están los dos en C#, así que una referencia de proyecto «se ve limpia». Ata los dos procesos | ¿El `.csproj` del front referencia el de la API? No debe |
| **Un `ApiService` genérico** con la tabla como parámetro | Es más corto, y con una sola tabla ni se nota | ¿El servicio se llama `ServicioAreaConocimiento` o `ApiService`? |
| **Meter Bootstrap por CDN** | Es lo que hace todo el mundo | ¿`App.razor` tiene un `<link>` a un dominio externo? |

**Y una pregunta que hay que hacerle siempre, porque no la contesta sola:**

> «Apaga la API con `docker compose stop api-investigacion` y dime qué muestra
> la pantalla.»

Si la respuesta no es «sigue en pie, con un aviso y sin datos», el front está
leyendo de donde no debe — o no maneja el caso de que la API no responda, que
es el mismo problema visto de otro lado.


---

## Antes de abrir el chat: prepare SU proyecto

**Ojo: NO se construye dentro de la carpeta clonada.** El repositorio clonado
es el **material de referencia**; su trabajo de reconstrucción va en una
**carpeta nueva y vacía**, fuera de él.

### 1. La carpeta y las subcarpetas

Cree la carpeta de su proyecto, ábrala en VS Code (*File → Open Folder*) y, en
la terminal integrada (*Terminal → New Terminal*, PowerShell), parado en ella:

```powershell
mkdir docs\spec_kit\versiones\v1_area_conocimiento, db, api_investigacion, api_investigacion\Controllers, api_investigacion\Excepciones, api_investigacion\Modelos, api_investigacion\Peticiones, api_investigacion\Repositorios, api_investigacion\Servicios, api_investigacion\pruebas, front_blazor, front_blazor\Components, front_blazor\Components\Layout, front_blazor\Components\Pages, front_blazor\Servicios, front_blazor\wwwroot, pruebas_humo
```

### 2. Los archivos VACÍOS

**Usted los va llenando** uno a uno, pegando en cada uno el código que la IA le
entregue. Que nazcan vacíos y con su nombre puesto es lo que le da forma al
trabajo: se ve de una vez cuántas piezas son y dónde va cada una.

```powershell
New-Item .gitattributes, .gitignore, api_investigacion\ApiInvestigacion.csproj, api_investigacion\Controllers\AreaConocimientoController.cs, api_investigacion\Dockerfile, api_investigacion\Excepciones\NoEncontradoExcepcion.cs, api_investigacion\Modelos\AreaConocimiento.cs, api_investigacion\Peticiones\AreaConocimientoActualizar.cs, api_investigacion\Peticiones\AreaConocimientoCrear.cs, api_investigacion\Peticiones\AreaConocimientoReemplazo.cs, api_investigacion\Program.cs, api_investigacion\Repositorios\IRepositorioAreaConocimiento.cs, api_investigacion\Repositorios\RepositorioAreaConocimientoSqlServer.cs, api_investigacion\Servicios\IServicioAreaConocimiento.cs, api_investigacion\Servicios\ServicioAreaConocimiento.cs, api_investigacion\appsettings.json, api_investigacion\pruebas\Programa.cs, api_investigacion\pruebas\PruebaCapas.csproj, docker-compose.yml, front_blazor\Components\App.razor, front_blazor\Components\Layout\MainLayout.razor, front_blazor\Components\Layout\NavMenu.razor, front_blazor\Components\Pages\AreasDeConocimiento.razor, front_blazor\Components\Pages\Home.razor, front_blazor\Components\Routes.razor, front_blazor\Components\_Imports.razor, front_blazor\Dockerfile, front_blazor\FrontInvestigacion.csproj, front_blazor\Program.cs, front_blazor\Servicios\ServicioAreaConocimiento.cs, front_blazor\appsettings.json, front_blazor\wwwroot\app.css, pruebas_humo\humo_front.py
```

> **Fíjese en lo que la lista tiene y en lo que no.**
>
> **Tiene** los archivos del front —`front_blazor\…`— porque **la versión
> incluye su pantalla** (Artículo 1.1). Son la mitad del trabajo, no un
> añadido, y por eso nacen vacíos junto a los de la API.
>
> **No tiene** nada de `db\`: esos no nacen vacíos, se copian (paso 3).

### 3. Los archivos que vienen DADOS: cópielos del repositorio del curso

Con el explorador de Windows (Ctrl+C, Ctrl+V), cada uno a la misma ruta:

| Del clon del curso | A su proyecto |
|---|---|
| `db\init.sh` | `db\` |
| `db\investigacion.sql` | `db\` |
| `docs\spec_kit\1_constitution.md` | `docs\spec_kit\` |
| Los `.md` de `docs\spec_kit\versiones\v1_area_conocimiento\` | la misma ruta |

Estos vienen dados y **la IA no los genera**: los documentos se le SUBEN al
chat, y los scripts de `db\` son la base de datos ya escrita.

### 4. Compruebe antes de empezar

- [ ] `docs\spec_kit\1_constitution.md` existe y tiene contenido.
- [ ] `docs\spec_kit\versiones\v1_area_conocimiento\` tiene **9 archivos**.
- [ ] `db\` tiene sus scripts **con contenido**, no vacíos.
- [ ] `front_blazor\` existe con sus carpetas, aunque los archivos estén
      vacíos: si no está, la versión va a nacer sin la mitad que se ve.

Si algo está vacío o falta, es el paso 3.

> **La estructura queda lista ANTES de hablar con la IA**, y es la que describe
> `3_plan.md`. Así el chat entrega código para archivos que ya existen, en vez
> de proponerle a usted dónde ponerlos.
