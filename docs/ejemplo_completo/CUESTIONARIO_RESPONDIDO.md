# Entrevista con el cliente — Recolección de residuos electrónicos

**Grupo 580202009-8 · Aplicación y Servicios Web · ITM**

| | |
|---|---|
| **Fecha de la entrevista** | 3 de septiembre de 2026 |
| **Entrevistados** | Marcela Ruiz (coordinadora ambiental), Julián Restrepo (jefe de operaciones), Diana Gómez (auditora) |
| **Método** | Cuatro rondas — ver `CONCEPTOS_ELICITACION.md` |

> **Esto es el insumo de las historias de usuario.** Cada historia del
> documento «Historias de usuario» sale de alguna de estas respuestas, y
> al final de este documento está la tabla que muestra de cuál.
>
> Las personas son **inventadas**, como todos los datos del caso.

---

## Ronda 1 — Por qué existe esto

**P: ¿Qué problema le está costando plata, tiempo o disgustos hoy?**

> **Marcela:** Que no sabemos cuánto recogemos. Cada punto de acopio lleva
> su cuaderno, y cuando la autoridad ambiental nos pide el informe
> trimestral, nos toca sumar a mano lo que cada operario anotó. El último
> informe lo entregamos ocho días tarde y con dos cifras que no cuadraban.

**P: ¿Cómo lo resuelven ahora, sin sistema?**

> **Julián:** Cuadernos y un Excel que consolida uno de la oficina. El
> problema no es el Excel, es que la información llega tarde y a veces no
> llega: si el operario se enferma, esa semana no hay datos.

**P: Si esto funciona, ¿qué número cambia? ¿Cuál es hoy y cuál quisiera?**

> **Marcela:** Hoy nos demoramos entre seis y ocho días armando el informe
> trimestral. Quisiéramos sacarlo el mismo día. Y hoy tenemos registrado
> más o menos el 60 % de lo que de verdad se recoge; el resto se pierde en
> el camino.

**P: ¿Qué pasa si no hacen nada?**

> **Marcela:** Nos arriesgamos a que nos suspendan la certificación. Y sin
> certificación, las empresas grandes no nos entregan sus residuos.

**P: ¿Hay algo que hoy funcione bien y no quieran perder?**

> **Julián:** El certificado que se le entrega al ciudadano cuando deja
> algo. Es un papelito, pero la gente lo guarda y vuelve por eso.

---

## Ronda 2 — Quiénes

**P: ¿Quién va a usar esto día a día? Nómbrelos por su oficio.**

> **Julián:** El **operario del punto**, que recibe y pesa. El
> **coordinador** —Marcela— que consulta y saca informes. Y el
> **ciudadano**, que llega con sus aparatos; él no toca el sistema, pero
> sus datos sí quedan.

**P: ¿Y los gestores? ¿Ellos entran al sistema?**

> **Julián:** Sí, cada gestor administra sus puntos. Nosotros les
> registramos la resolución ambiental que los habilita, porque sin
> resolución no pueden recoger nada.

**P: ¿Quién decide, quién ejecuta, quién solo consulta?**

> **Marcela:** Yo decido qué tipos de residuo se reciben y con qué
> peligrosidad se manejan. El operario ejecuta. La auditora —Diana— solo
> consulta, pero es la que más pregunta.

**P: ¿Quién sale perjudicado si esto se hace?**

> **Julián:** El operario, al principio: hoy anota en treinta segundos y
> con el sistema le va a tomar más. Si el registro es lento, lo van a
> saltar y volvemos al cuaderno.

**P: ¿Alguien más necesita ver esto?**

> **Diana:** Yo. Y necesito poder mirar hacia atrás: los kilos que se
> reportaron hace un año tienen que seguir ahí, aunque el gestor ya no
> exista.

---

## Ronda 3 — El día de trabajo

**P: Cuénteme un día normal en un punto de acopio.**

> **Julián:** El operario abre, revisa que los contenedores tengan
> espacio. Llega un ciudadano con una caja: pantallas viejas, un par de
> celulares, cables. El operario separa por tipo, pesa cada tipo, anota, y
> le entrega el certificado con el número de constancia.

**P: ¿Una entrega trae siempre un solo tipo?**

> **Julián:** Casi nunca. La gente llega con lo que tenga en la casa. Una
> entrega normal trae dos o tres tipos distintos, cada uno con su peso.

**P: ¿En qué paso pierde más tiempo?**

> **Julián:** En pesar y anotar. Y en buscar en el cuaderno si el
> ciudadano ya había venido antes.

**P: ¿Qué es lo que más se equivoca?**

> **Julián:** El peso, y el tipo. A veces anota «celulares» lo que en
> realidad son baterías, y eso importa: la batería es de alta
> peligrosidad y se maneja distinto.

**P: ¿Qué hace cuando algo sale mal?**

> **Julián:** Llama a la oficina y allá corrigen el Excel. Por eso quiero
> que el sistema deje corregir sin tener que borrar todo y volver a
> empezar.

**P: ¿Y cuando un gestor se retira?**

> **Julián:** Le vence la resolución y deja de operar. Lo sacamos del
> listado para que nadie le asigne puntos nuevos. Pero ojo con eso —

> **Diana:** — eso es lo que yo decía. Sáquenlo del listado, pero las
> entregas que recibió el año pasado **tienen que seguir apareciendo en
> los informes**. Esos kilos ya se reportaron.

---

## Ronda 4 — Las reglas y los ejemplos

**P: Sobre los tipos de residuo. ¿Qué tiene que pasar siempre?**

> **Marcela:** Todo tipo tiene un nombre, una peligrosidad y un valor por
> kilo. La peligrosidad es alta, media o baja, no hay más. Y el valor por
> kilo es del tipo, no de cada entrega: si hoy la batería de litio está a
> 3.800 el kilo, está a 3.800 en todos los puntos.

**P: Deme un ejemplo concreto.**

> **Marcela:** «Baterías de litio», peligrosidad alta, 3.800 pesos el
> kilo. «Cables y conectores», baja, 900.

**P: ¿Y si alguien intenta crear un tipo sin valor por kilo?**

> **Marcela:** No debería dejarlo. Sin ese valor no podemos liquidarle
> nada al ciudadano.

**P: ¿El valor cambia?**

> **Marcela:** Sí, con el precio de la chatarra. Por eso necesito poder
> cambiar **solo ese número**, sin volver a escribir el nombre y la
> peligrosidad.

**P: Sobre los gestores. ¿Qué es obligatorio?**

> **Julián:** El nombre, el NIT y **la resolución**. Sin resolución no
> existe para nosotros.

**P: ¿Puede haber un gestor sin puntos de acopio?**

> **Julián:** Sí, y pasa siempre: primero firman el convenio y semanas
> después abren el primer punto.

**P: Sobre los ciudadanos.**

> **Marcela:** Nombre, apellidos y documento. El documento es obligatorio
> porque el certificado va a nombre de alguien.

**P: ¿Y si alguien se registra y nunca entrega nada?**

> **Marcela:** Se queda registrado, no pasa nada.

**P: Cuando el listado tenga miles de registros, ¿cómo lo consultan?**

> **Diana:** No quiero esperar a que carguen diez mil filas para ver diez.
> Que se pueda pedir un pedazo, y que me diga cuántos me trajo.

**P: ¿Qué no puede pasar nunca?**

> **Diana:** Que se pierda un dato reportado. Y que el sistema acepte algo
> incompleto: si un campo obligatorio llega vacío, que lo rechace y diga
> **cuál** campo es, no un error genérico. Con esto se emiten
> certificados.

**P: ¿Cómo sabemos que quedó bien hecho?**

> **Marcela:** Cuando el operario pueda registrar una entrega completa,
> con sus dos o tres tipos, en menos tiempo del que le toma escribirla en
> el cuaderno.

---

## Lo que quedó sin responder

Se anota, no se rellena a ojo. En el spec kit va como
`[NECESITA ACLARACIÓN]`:

| # | Pregunta que nadie supo responder |
|---|---|
| 1 | **¿Quién asigna el identificador de cada registro?** Nadie lo pensó. Marcela dijo «que el sistema lo ponga», pero el script de la base no lo genera solo. |
| 2 | **¿Se puede eliminar un tipo de residuo que ya tiene entregas registradas?** Marcela dice que nunca ha pasado. |
| 3 | **¿Qué hace la pantalla cuando no hay ningún registro activo?** No se habló. |

---

## De la entrevista a las historias

Cada historia del documento «Historias de usuario» sale de aquí:

| Historia | Sale de |
|---|---|
| **1. Registrar los tipos de residuo** | Ronda 4: nombre, peligrosidad, valor por kilo; el ejemplo de las baterías; «sin valor por kilo no podemos liquidar» |
| **2. Corregir un tipo de residuo** | Ronda 4: «necesito cambiar solo ese número, sin volver a escribir el resto» |
| **3. Registrar los gestores autorizados** | Ronda 2 y 4: la resolución ambiental; «puede haber gestor sin puntos» |
| **4. Sacar un gestor del sistema** | Ronda 3: Julián lo saca del listado; Diana exige conservar el histórico |
| **5. Registrarme para entregar mis residuos** | Ronda 4: documento obligatorio porque el certificado va a nombre de alguien |
| **6. Que un listado largo no tumbe la pantalla** | Ronda 4: «no quiero esperar diez mil filas para ver diez» |
| **7. Que los datos no se corrompan** | Ronda 4: «que rechace lo incompleto y diga cuál campo»; «con esto se emiten certificados» |
| **8. Ver y editar desde una pantalla** | Ronda 3: el operario pierde tiempo anotando y buscando en el cuaderno |

**Y el «para qué» de todas** sale de la ronda 1: el informe trimestral que
hoy toma ocho días y debería salir el mismo día, y el 40 % de lo recogido
que hoy no queda registrado.

---

## Lo que este documento NO resuelve

La entrevista dejó **tres preguntas abiertas** y **una tensión sin
resolver**: Julián quiere «eliminar» al gestor que se retira y Diana
exige que sus datos no se pierdan. Las dos cosas no pueden ser literales
al mismo tiempo.

**Esa tensión es real y es del cliente, no del documento.** Resolverla
—decidiendo qué significa «eliminar» en este sistema— es trabajo de quien
escribe la especificación.
