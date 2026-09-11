# Un caso completo, de la entrevista al cronograma

**Esto no es una evaluación de este curso.** Es un **ejemplo trabajado**,
para que vea cómo se encadena todo lo que los documentos conceptuales
explican por separado.

El caso es una red de recolección de residuos electrónicos: gestores
autorizados que operan puntos de acopio donde los ciudadanos entregan sus
aparatos viejos. Nada de esto es real: las personas, las empresas y los
datos son inventados.

---

## Léalo en este orden

| # | Documento | Qué muestra |
|---|---|---|
| 1 | [`CUESTIONARIO_RESPONDIDO.md`](CUESTIONARIO_RESPONDIDO.md) | **La entrevista con el cliente**, en cuatro rondas y respondida. Al final, la tabla que dice de qué respuesta sale cada historia |
| 2 | [`HISTORIAS_DE_USUARIO.md`](HISTORIAS_DE_USUARIO.md) | **Las ocho historias** que salieron de esa entrevista, con sus criterios de aceptación |
| 3 | [`EVALUACION_INDIVIDUAL.md`](EVALUACION_INDIVIDUAL.md) | El enunciado: problema, contexto, **MER en notación de Chen**, modelo relacional normalizado y el script de la base |
| 4 | [`PLAN_DE_DESARROLLO.md`](PLAN_DE_DESARROLLO.md) | **El plan**: fases, entregables, restricciones, y la metodología SDD explicada |
| 5 | [`CRONOGRAMA.xlsx`](CRONOGRAMA.xlsx) | El cronograma, con horas estimadas y una columna para las reales |

Los conceptos que hay detrás de cada uno están en la carpeta de arriba:
`CONCEPTOS_ELICITACION.md`, `CONCEPTOS_HISTORIAS_DE_USUARIO.md` y
`CONCEPTOS_PLAN_DE_DESARROLLO.md`.

---

## Qué mirar mientras lo lee

**La trazabilidad.** Cada historia se puede rastrear hasta una frase que
dijo alguien en la entrevista. Si una historia no se puede rastrear, es
que alguien la inventó — y eso pasa más de lo que parece.

**Los criterios que pueden fallar.** Ninguno dice «debe ser rápido».
Todos dicen un número o un código de estado: 204 si no hay filas activas,
404 al segundo borrado, 422 si falta un campo.

**Lo que quedó sin responder.** La entrevista cierra con tres preguntas
que nadie supo contestar. No se rellenaron a ojo: se anotaron, y en el
spec kit se escriben como `[NECESITA ACLARACIÓN]`.

**La contradicción del cliente.** En la ronda 3, uno de los entrevistados
pide «eliminar» al gestor que se retira y otra exige que sus datos no se
pierdan. Las dos cosas no pueden ser literales a la vez, y el documento
**no la resuelve a escondidas**: la deja señalada, porque resolverla es
trabajo de quien escribe la especificación.

Eso último es lo más parecido a un proyecto real que hay en esta carpeta.
