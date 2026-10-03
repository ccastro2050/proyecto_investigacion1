# Qué se espera de la VERSIÓN 2 del proyecto de aula

> Para el **equipo**. Qué tiene que estar listo al entregar la v2, cómo se
> comprueba y qué se entrega.
>
> **Entrega: 20%** — 10% de sustentación individual
> (incluidos sus commits) + 10% de entrega en equipo.
>
> **La fecha la fija su curso:** este repositorio es un ejemplo para las dos
> universidades, y cada una tiene su calendario.
>
> El método, el calendario y la rúbrica están en
> [0_METODOLOGIA.md](0_METODOLOGIA.md). Esto es **el detalle de la v2**.
>
> **Su stack:** la API en **C# / ASP.NET Core** sobre **SQL Server**.
> **El front es a libre elección del equipo** — Blazor, Flask, React, lo que
> decidan y sepan sostener. Lo que no es libre es que haya uno: una versión
> no está cerrada si la API responde y la interfaz no.

---

## 0. Lo que queda al terminar

**Todas las tablas de su módulo operables desde la interfaz gráfica** —menos
las del control de acceso, que son de la v3—, con:

- **todo el CRUD pasando por procedimientos almacenados**,
- **al menos un disparador** funcionando,
- **ninguna clave foránea digitada a mano**,
- y los criterios de la v1 **todavía en verde**.

---

## 1. Primero se cierra la v1

**La v1 son las tablas SIN clave foránea de su módulo**, y el ejemplo que les
entregó el profesor construye **una sola**, de punta a punta, para mostrar el
molde. Las demás son del equipo.

> **Por qué importa el orden.** Una tabla con clave foránea no se puede llenar
> si la tabla a la que apunta está vacía. Empezar la v2 sin cerrar la v1 es
> construir desplegables que no tienen de dónde sacar opciones.

Cuáles son: la fila **v1** de `docs/spec_kit/versiones/0_mapa_versiones.md`
**de su repositorio**.

---

## 2. El alcance de la v2

**Las tablas CON clave foránea de su módulo**, y entre ellas varias **tablas
puente** —clave primaria compuesta, dos claves foráneas—.

> Las tablas no se copian aquí: el mapa de **su** repositorio es el único sitio
> donde están, y así este documento no miente el día que el mapa cambie.

---

## 3. Las relaciones MAESTRO-DETALLE

Las que cada módulo ya tiene en su esquema. **Busque las del suyo.**

| Módulo | Maestro | Su detalle |
|---|---|---|
| Gestión Profesoral | **`docente`** | estudios_realizados · evaluacion_docente · experiecia · reconocimiento |
|  | **`estudios_realizados`** | apoyo_profesoral · beca |
| Innovación Curricular | **`programa`** | acreditacion · activ_academica · pasantia · premio · registro_calificado |
|  | **`universidad`** | facultad |
|  | **`facultad`** | programa |
| Investigación | **`universidad`** | grupo_investigacion |
|  | **`grupo_investigacion`** | semillero |
|  | **`linea_investigacion`** | docente |
| Mapa de Conocimiento | **`proyecto`** | producto |
|  | **`tipo_producto`** | producto |
|  | **`linea_investigacion`** | docente |
| Cátedras | **`asistente`** | ponencia · documento_asistente · consentimiento_datos · clave_acceso |
|  | **`encuesta`** | pregunta · respuesta_encuesta · sesion |
|  | **`sesion`** | ponencia · enlace_registro |

> **Qué significa que algo sea detalle.** Un `estudios_realizados` **no existe
> sin su `docente`**. No se crea suelto y después se le busca padre.

**Lo que se espera en la interfaz —sea cual sea la que elijan—:** al abrir un maestro, ver **su detalle ahí
mismo** y poder agregarle renglones sin salir de la pantalla. No un menú aparte
donde haya que volver a elegir de qué maestro se trata.

> **Y cuando se registran varios renglones de una, van en UN SOLO ENVÍO.** Tres
> renglones no son cuatro peticiones: si la tercera fallara quedaría medio
> registro guardado — y «medio» no es un estado que el negocio reconozca.

---

## 4. TODO el CRUD con procedimientos almacenados

**Listar, consultar, crear, modificar y eliminar: los cinco, de todas las
tablas, pasan por un procedimiento almacenado.**

| | |
|---|---|
| **Qué va en la base** | El SQL: los `SELECT`, los `INSERT`, los `JOIN` de los listados |
| **Qué va en su repositorio** | La **llamada** al procedimiento, con sus parámetros |
| **Qué NO va en su repositorio** | SQL escrito a mano, y mucho menos armado concatenando texto |

> **Por qué.** El SQL queda en **un solo sitio**, con nombre, versionado en el
> script de la base. Y los parámetros viajan como parámetros: una consulta
> armada pegando texto es por donde entra una inyección de SQL.

### Listar

```sql
CREATE OR ALTER PROCEDURE sp_listar_docente
AS
BEGIN
    SET NOCOUNT ON;
    -- si la tabla tiene `activo`, EL LISTADO LO FILTRA
    SELECT * FROM docente WHERE activo = 1 ORDER BY cedula;
END;
```

### Crear — devolviendo la fila nueva

```sql
CREATE OR ALTER PROCEDURE sp_crear_docente
    @cedula INT,
    @nombres VARCHAR(200),
    @apellidos VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO docente (cedula, nombres, apellidos)
    VALUES (@cedula, @nombres, @apellidos);
    -- devuelve la fila COMO QUEDO GUARDADA
    SELECT * FROM docente WHERE cedula = @cedula;
END;
```

> **El procedimiento devuelve la fila COMO QUEDÓ GUARDADA**, no como la mandó
> el formulario. Así su API responde con los valores por defecto que puso la
> base y con lo que el disparador haya calculado.

**Un nombre por operación y por tabla**, para que se encuentren:
`sp_listar_<tabla>`, `sp_consultar_<tabla>`, `sp_crear_<tabla>`,
`sp_actualizar_<tabla>`, `sp_eliminar_<tabla>`.

### Y antes de escribir el de eliminar: ¿su borrado es lógico?

**Mire su esquema.** Si sus tablas tienen una columna `activo`, el borrado es
**lógico**: `sp_eliminar_<tabla>` hace un `UPDATE ... SET activo = 0`, **no un
`DELETE`**, y **todos los listados filtran los inactivos**.

> No es igual en todos los módulos: en los cuatro módulos de docencia casi
> todas las tablas la tienen; en Cátedras, menos de la mitad. **Lo que mande es
> su esquema**, y si su tabla no tiene `activo` y usted cree que debería, eso
> es una decisión de la v2 — tómenla y escríbanla en el `2_spec.md`.

---

## 5. AL MENOS UN DISPARADOR — elija uno de estos

**Se exige mínimo uno, funcionando y comprobable.** Cinco propuestas; **elija
al menos una**, o proponga la suya si su módulo pide otra cosa.

| | Propuesta | Qué resuelve |
|---|---|---|
| **A** | **Contador en el maestro**: `docente.total_estudios` se mantiene solo | Saber cuántos hijos tiene sin contar en cada consulta |
| **B** | **Sello de modificación**: `fecha_modificacion` se pone sola en cada `UPDATE` | Nadie se puede olvidar de actualizarla |
| **C** | **Bitácora**: lo que se retira queda copiado en una tabla `bitacora` | Saber qué se retiró y cuándo |
| **D** | **Validación que un `CHECK` no puede hacer**: que la fecha del detalle caiga dentro del rango del maestro | Un `CHECK` solo ve su propia fila; esto mira otra tabla |
| **E** | **Estado derivado**: marcar el maestro como «con producción» al recibir su primer detalle | Un dato que se calcula, no que alguien recuerde marcar |

### La propuesta A, completa

```sql
ALTER TABLE docente ADD total_estudios INT DEFAULT 0;
GO

CREATE OR ALTER TRIGGER trg_contar_estudios
ON estudios_realizados
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE d SET d.total_estudios = (
        SELECT COUNT(*) FROM estudios_realizados e
        WHERE e.docente = d.cedula AND e.activo = 1
    )
    FROM docente d
    WHERE d.cedula IN (SELECT docente FROM inserted
                       UNION SELECT docente FROM deleted);
END;
```

> **Fíjese en que escucha el `INSERT`, el `UPDATE` **y** el `DELETE`.** Si su
> borrado es lógico, retirar un hijo es un `UPDATE` — y un disparador que solo
> oiga el `INSERT` y el `DELETE` **no se entera**. El contador queda mal justo
> cuando alguien retira algo, que es el caso que nadie prueba.

**Cómo se comprueba:** se hace la operación **por la interfaz** y se mira que
el dato cambió **sin que la API lo haya enviado**. Si su código manda el total,
el disparador no está demostrando nada.

---

## 6. Las claves foráneas NO se digitan

**Nunca** un campo de texto donde va un código. Un desplegable **cargado desde
la API**, que **muestra el nombre** y **manda la clave**.

> **Por qué.** Un campo de texto obliga a la persona a adivinar qué códigos
> existen. Escribe uno que no está, el servidor responde un error, y **no hay
> forma de saber cuál era el bueno**.

### El ejemplo

Al crear un **`estudios_realizados`** hay que decir de qué **`docente`** es.

| | |
|---|---|
| **Lo que la persona VE** | `nombres` y `apellidos` del docente |
| **Lo que se MANDA** | `cedula`, la clave |
| **De dónde salen las opciones** | De la API: `GET /api/docente` |

```html
<!-- el desplegable: muestra el nombre, manda la clave -->
<select name="docente">
  <option value="">— seleccione —</option>
  <!-- una opción por cada fila que devolvió GET /api/docente:
       el TEXTO es lo que la persona lee, el value es lo que viaja -->
  <option value="1017245">Ana Torres Gómez</option>
  <option value="1098332">Carlos Pérez Mejía</option>
</select>
```

Si la persona elige **Ana Torres Gómez**, lo que viaja es su clave:

```json
{
  "docente": 1017245
}
```

> **Eso es lo que hay que ver:** en la pantalla se lee «Ana Torres Gómez»; en
> la petición viaja `1017245`. La persona reconoce nombres; la base necesita
> claves.
>
> **Y si su borrado es lógico, el desplegable solo ofrece los ACTIVOS**, porque
> el listado del que sale ya los filtra. Ofrecer un padre retirado es ofrecer
> una opción que la base va a rechazar.

**Si la clave foránea es opcional**, el desplegable lleva una opción
**«(ninguna)»** que manda `null` — que **no** es lo mismo que cadena vacía. Una
cadena vacía donde va un número hace que el servidor responda un error de
conversión en inglés, en vez de aceptar que no hay valor.

---

## 7. La integridad referencial: 409, nunca 500

| Qué hace alguien | Qué responde la base | Qué tiene que responder su API |
|---|---|---|
| Crear un hijo cuyo **padre no existe** | Rechaza por clave foránea | **409** con mensaje en castellano |
| Repetir **la misma pareja** en una tabla puente | Rechaza por clave primaria duplicada | **409** |
| Borrar un padre con hijos **(solo si su borrado es físico)** | Rechaza por clave foránea | **409** |

> Si responde **500**, lo que está diciendo es «me caí», y quien lo lea va a
> buscar un error que no existe.

**Y una decisión que el equipo tiene que tomar y escribir:** si su borrado es
**lógico**, ¿se puede **retirar** un maestro que todavía tiene detalle activo?
La base no lo impide —es un `UPDATE`—, así que es una **regla de negocio**
suya. Decídanla, pónganla en el servicio, y escríbanla en el `2_spec.md`.

---

## 8. Los criterios de aceptación

| # | Criterio | Cómo se comprueba |
|---|---|---|
| **1** | Las tablas de la v1 tienen CRUD completo y su interfaz | Se abre cada una: crear, editar, retirar |
| **2** | Las tablas de la v2, también | Lo mismo, una por una |
| **3** | **Los cinco verbos de todas las tablas pasan por procedimientos almacenados** | Se abre el repositorio: **no hay SQL escrito a mano** |
| **4** | **Al menos un disparador funciona** | Se hace la operación por la interfaz y el dato cambia solo (§5) |
| **5** | Cada clave foránea se elige de un **desplegable cargado de la API** | El desplegable está **lleno** y muestra nombres, no códigos |
| **6** | La clave foránea opcional acepta **«(ninguna)»** → `null` | Se crea un registro sin ella |
| **7** | Crear un hijo con un padre inexistente responde **409** en castellano | Desde la colección de pruebas. **Un 500 es criterio fallado** |
| **8** | Las tablas puente se asignan y se retiran **con sus dos claves**, y repetir la pareja da **409** | Se asigna, se repite y se retira |
| **9** | El detalle se ve y se agrega **desde el maestro** | Se abre un maestro y ahí está su detalle |
| **10** | Si el borrado es lógico, **los listados filtran** | Se retira un registro: desaparece de la lista, **sigue en la base** |
| **11** | La interfaz **no habla en jerga** | No dice «PUT», «PATCH», «422» ni «FK» en ninguna pantalla |
| **12** | **La regresión de la v1 pasa completa** | §9 |

---

## 9. La regresión, que es obligatoria

**Los criterios de la v1, completos, el día de la entrega.** No «deberían
seguir funcionando»: se corren.

> **La v2 incluye la v1.** No la reemplaza: lo construido sigue en pie, con su
> código y su interfaz, y sus criterios se vuelven a correr.

---

## 10. Lo que NO es de la v2

| | Es de la |
|---|---|
| Iniciar sesión, contraseñas, tokens, permisos por rol | **v3** |
| CRUD de `usuario`, `rol` y `rol_usuario` | **v3** |
| Consultas multitabla, dashboard con gráficos | **v4** |
| Imagen corporativa, páginas corporativas, publicación | **v4** |

> **Adelantarlos no suma, resta.** Una v2 que ya trae media sesión le quita a
> la v3 la mitad de su razón de ser. Si le sobra tiempo, cierre bien lo de la
> v2 — empezando por los criterios 4 y 7.

---

## 11. Qué se entrega

| | |
|---|---|
| **El spec kit de la v2** | `docs/spec_kit/versiones/v2_<nombre>/` con los mismos nueve documentos de la v1 **y la guía de IA**. Describe **solo el delta** |
| **El script de la base** | Con **los procedimientos** y **el disparador**, versionados ahí |
| **El código** | Repositorios que **llaman** a los procedimientos |
| **La interfaz gráfica** | Las pantallas de los recursos nuevos. **Una versión no está cerrada si la API responde y la interfaz no** |
| **La colección de pruebas** | Con las peticiones nuevas, **incluidas las que tienen que fallar** |
| **LOS PROMPTS** | Los que **de verdad usaron** para generar el código a partir del spec kit — por chat o con el IDE agéntico—, **con lo que tuvieron que corregirle a la IA**. Van en la carpeta de la versión, al lado de su guía de IA |
| **El tag `v2`** | Sobre el commit que pasa los doce criterios |

> **El spec kit se escribe ANTES.** Si se escribe al final es un informe de lo
> que se hizo, y entonces no sirvió para decidir nada. Las tres compuertas
> están en [0_METODOLOGIA.md](0_METODOLOGIA.md) §3.1.

> **Y los prompts se entregan SIEMPRE**, en las dos modalidades: el del chat y
> el del IDE agéntico. No es burocracia — es el eslabón del medio.
>
> El spec kit dice **qué** construir. El código es **lo construido**. El prompt
> es **cómo se pasó de uno al otro**: entregar los dos extremos y no el medio
> es entregar un resultado sin su procedimiento.
>
> **Y lo que más vale es lo que tuvieron que corregirle a la IA.** Si pidieron
> algo y salió mal, la corrección que lo arregló dice más del equipo que el
> código final — y en la sustentación individual es lo que distingue a quien
> dirigió el trabajo de quien pegó una respuesta.

---

## 12. Las trampas de esta versión

| | Qué pasa | Cómo se nota |
|---|---|---|
| **El disparador sordo al `UPDATE`** | Con borrado lógico, retirar es un `UPDATE`. Un disparador que solo oye `INSERT` y `DELETE` no se entera | El contador queda mal cuando alguien retira |
| **El listado que no filtra** | Se olvidó el `WHERE activo` | Lo retirado sigue apareciendo |
| **El desplegable con inactivos** | El listado que lo llena no filtra | Se puede elegir un padre retirado |
| **El 500 que debía ser 409** | No se tradujo el error de la base | Criterio 7 |
| **El sobre de la respuesta** | La API devuelve `{tabla, limite, total, datos}`, no una lista pelada. Leerlo mal deja la pantalla **vacía sin ningún error** | Una tabla sin filas y sin mensaje |
| **La tabla puente con botón de editar** | Una pareja existe o no existe; no se edita | — |
| **El procedimiento que no devuelve la fila** | La interfaz no puede mostrar lo que acaba de crear | — |

---

## Dónde está cada cosa

| | |
|---|---|
| El método, el calendario y la rúbrica | [0_METODOLOGIA.md](0_METODOLOGIA.md) |
| Las tablas de cada versión | `docs/spec_kit/versiones/0_mapa_versiones.md` **de su repositorio** |
| Lo que su módulo tiene que hacer | los `modulo_*.md` de esta misma carpeta |
| Cómo encajan los módulos | [proyecto_completo.md](proyecto_completo.md) |
