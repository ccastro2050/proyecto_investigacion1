# Lista de chequeo de requisitos — Versión 1

> **La compuerta 3** del método. Esta lista revisa **la ESPECIFICACIÓN, no
> el código**: se pasa cuando los documentos 2 a 8 están escritos y ANTES
> de programar la primera línea.

## Cómo se usa

- **Las casillas las marca una persona.** Una IA puede ayudar a evaluar,
  pero no puede auto-aprobarse: quien firma responde por la versión.
- Se marca `[x]` solo cuando el criterio se cumple **hoy, en el
  documento** — no "cuando lo arregle".
- **Con una sola casilla en rojo no se escribe código.**
- En equipo: cada quien revisa con la lista la parte que escribió otro.

---

## A. Claridad

- [ ] Ningún requisito usa palabras sin definir (*rápido, amigable,
      correcto, adecuado*).
- [ ] No queda ningún `[NECESITA ACLARACIÓN: …]` sin resolver en la
      sección 6 de [2_spec.md](2_spec.md).
- [ ] Cada RF explica UNA cosa.
- [ ] Los RF no mencionan Dapper, SQL Server ni nombres de clase: el QUÉ
      está separado del CÓMO.

## B. Medible

- [ ] Los **7 criterios** dicen un valor concreto: 218, 3, `1A01`, 404,
      422, 500.
- [ ] Cada criterio tiene su comando en [7_quickstart.md](7_quickstart.md),
      **con el mismo número**.
- [ ] Los errores están dichos por su número, no como "responde con error".
- [ ] El contraste `PUT` 422 vs `PATCH` 200 está escrito como criterio.
- [ ] El borrado lógico es **verificable**: el criterio 5 comprueba que el
      total vuelve a 218 y que la fila sigue con `activo = 0`.

## C. Completitud

- [ ] Los 7 RF cubren los 5 verbos, el listado con límite y el
      diagnóstico.
- [ ] [2_spec.md](2_spec.md) tiene su **NO incluye** explícito, y ahí están
      las FK, el JWT, el front y la reactivación.
- [ ] Cada endpoint de [6_contracts.md](6_contracts.md) documenta sus
      desenlaces de **ERROR**, no solo el feliz.
- [ ] [5_data_model.md](5_data_model.md) trae los datos exactos de los que
      dependen los comandos: 218 filas, `1A01`, las seis grandes áreas.
- [ ] Cada decisión de [4_research.md](4_research.md) tiene su alternativa
      descartada.

## D. Coherencia

- [ ] Todo RF aparece en los contratos.
- [ ] Todo contrato tiene una tarea que lo construye en
      [8_tasks.md](8_tasks.md).
- [ ] Los ejemplos de los contratos usan datos que existen (`1A01` sí,
      `9Z99` no).
- [ ] [3_plan.md](3_plan.md) no nombra ningún archivo que ninguna tarea
      construya, ni al revés.
- [ ] El **Chequeo de constitución** de `3_plan.md` recorre los **11**
      artículos, sin saltarse ninguno.
- [ ] El `7_quickstart.md` dice **dónde están las credenciales**, como
      promete el Artículo 7.
- [ ] Los tipos de `5_data_model.md` coinciden con los del script
      `db/investigacion.sql` (`VARCHAR(6)`, `VARCHAR(150)`, `activo`).
      **Esta casilla se cierra en la Fase 0**, cuando el script exista: es
      la única de la lista que depende de algo construido.

## E. Alcance

- [ ] Ningún documento pide que el **código** de la v1 nombre otra tabla
      de las 19. (Explicarlas en una decisión sí vale: C1 menciona
      `ac_linea` porque el cambio de tipo la arrastra.)
- [ ] Ningún documento anticipa la v2, la v3 o la v4 (Artículo 1).
- [ ] Los paquetes que nombra `3_plan.md` son los tres que permite el
      Artículo 2.
- [ ] Ningún documento escribe la contraseña (Artículo 7).

---

## F. La pantalla — la mitad que no es la API

- [ ] La spec tiene un requisito **de la pantalla** (RF8), no solo de los
      endpoints.
- [ ] Está dicho que **una versión no está cerrada si la API responde y la
      pantalla no**.
- [ ] Hay un criterio que comprueba que la pantalla muestra **lo que dio la
      API**, no datos inventados ni de otra parte.
- [ ] Hay un criterio que se comprueba **apagando la API**: la pantalla sigue
      en pie, con su aviso y **sin un solo dato**.
- [ ] Está escrito que la pantalla **no le habla al usuario en jerga**, y cómo
      se comprueba —**sobre el texto visible**, no sobre el HTML—.
- [ ] Está escrito que el front **no comparte código** con la API, aunque los
      dos estén en C#, y qué pasaría si lo compartiera (D-v1-9).
- [ ] Está declarado **lo que la prueba automática NO puede hacer** con Blazor
      Server, y qué queda para el recorrido a mano.
- [ ] El front tiene **un servicio por recurso**, no uno genérico con el nombre
      de la tabla como parámetro.

## Resultado

| | |
|---|---|
| **Revisada por** | *(nombre de quien firma)* |
| **Fecha** | |
| **Casillas en rojo** | |
| **Veredicto** | ⬜ En verde: puede empezar el código · ⬜ En rojo: vuelve a la spec |
