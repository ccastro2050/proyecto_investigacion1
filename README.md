# Módulo Investigación — ejemplo de referencia

Este repositorio contiene **dos cosas distintas**, y conviene no
confundirlas:

| | Qué es |
|---|---|
| [`ProyectosDeAula/`](ProyectosDeAula/) | **El material del curso**: la metodología, los cuatro documentos de módulo y los scripts de base de datos. Es lo que ya conocen |
| Todo lo demás | **El ejemplo de referencia** del módulo Investigación: su versión 1, construida siguiendo esa metodología al pie de la letra |

El ejemplo no es un sistema para descargar: es un **molde de método**. Se
ejecuta, se estudia, y se reconstruye.

---

## 1. Arranque: un solo comando

Solo hace falta **Docker Desktop**. No hay que instalar .NET ni SQL Server.

```powershell
git clone https://github.com/ccastro2050/proyecto_investigacion1.git
cd proyecto_investigacion1
docker compose up -d --build
```

La primera vez tarda unos minutos: descarga las imágenes, espera a que el
motor **responda** (no solo a que exista), crea la base con sus 19 tablas y
sus catálogos, y compila la API. Al terminar:

| Qué | Dónde |
|---|---|
| **LA PANTALLA** (lo que ve el usuario) | http://localhost:8071 |
| Áreas de conocimiento | http://localhost:8071/areas-de-conocimiento |
| **API — diagnóstico** | http://localhost:8070/ |
| **Documentación interactiva** | http://localhost:8070/swagger |
| Listado de áreas de conocimiento | http://localhost:8070/api/area_conocimiento |
| SQL Server (SSMS o SQLTools, opcional) | `localhost,11470` · usuario `sa` |

**Pruebe la joya didáctica de la v1:** un `PUT` sin uno de los campos
responde **422**; el **mismo cuerpo** enviado por `PATCH` responde **200**.
Esa diferencia es parte de lo que enseña esta versión, y está en la
colección de [Postman](postman/) lista para probar con clics.

> **¿La contraseña de la base?** Está a la vista en el
> `docker-compose.yml`. Es una excepción **declarada** (Artículo 7 de la
> [constitución](docs/spec_kit/1_constitution.md)): este repositorio es una
> plantilla que corre en contenedores desechables y nunca se despliega.
> **En su proyecto de aula eso no se copia:** ahí los secretos van en un
> `.env` fuera de git, y un secreto quemado anula el criterio de seguridad
> de la versión.

### Los días siguientes

```powershell
docker compose up -d           # encender
docker compose down            # apagar (los datos se conservan)
docker compose down -v         # resetear la base a su estado original
```

Si edita un `.cs`, **no hay que hacer nada**: el código está montado y
`dotnet watch` recompila y reinicia solo.

## 2. Qué construye la versión 1

El CRUD completo de **`area_conocimiento`** —218 filas de catálogo— de
punta a punta: controlador, servicio, repositorio, interfaces, peticiones
por verbo y una prueba que corre **sin base de datos**.

Es **una** de las seis tablas sin clave foránea que pide la v1 del módulo.
Las otras cinco son **el mismo patrón** con otros nombres: el equipo que
tome este ejemplo lo revisa y, si está de acuerdo, lo retoma y lo completa;
si no, lo rehace a su manera. Lo que no puede es cambiar la especificación
sin pasar por sus compuertas.

## 3. Estructura

```
proyecto_investigacion1/
├── db/                                 el script y su inicializador (artefacto DADO)
├── api_investigacion/
│   ├── Controllers/                    CAPA 1: HTTP — códigos de estado y JSON
│   ├── Peticiones/                     la frontera de entrada: valida el cuerpo → 422
│   ├── Modelos/                        la entidad, lo que viaja entre capas
│   ├── Servicios/                      CAPA 2: negocio — no conoce HTTP ni el motor
│   ├── Repositorios/                   CAPA 3: datos — el SQL con Dapper
│   ├── Excepciones/                    cómo el negocio avisa un 404 sin hablar de HTTP
│   └── pruebas/                        el servicio con un repositorio de mentiras
├── docs/spec_kit/                      LA FUENTE DE VERDAD (ver abajo)
├── postman/                            los 14 endpoints listos para probar con clics
├── docker-compose.yml                  TODO el sistema declarado en un archivo
└── ProyectosDeAula/                    el material del curso
```

**La regla de lectura:** el sistema vive en `docker-compose.yml`, la API en
`api_investigacion/` (una carpeta por capa), y **todo lo que explica** vive
en `docs/`.

## 4. El spec kit: lo que se escribió ANTES del código

| Documento | Qué contiene |
|---|---|
| [SDD_SPECKIT.md](docs/SDD_SPECKIT.md) | **Empiece por aquí**: qué es SDD, qué es GitHub Spec Kit, y cómo se arman estos documentos |
| [1_constitution.md](docs/spec_kit/1_constitution.md) | Las 11 reglas permanentes del proyecto |
| [0_mapa_versiones.md](docs/spec_kit/versiones/0_mapa_versiones.md) | La ruta v1 → v4 y qué tabla entra en cada versión |
| [2_spec.md](docs/spec_kit/versiones/v1_area_conocimiento/2_spec.md) | QUÉ construir, los 7 criterios de aceptación y las **Clarificaciones** |
| [3_plan.md](docs/spec_kit/versiones/v1_area_conocimiento/3_plan.md) | CÓMO: capas, decisiones y el **Chequeo de constitución** |
| [4_research.md](docs/spec_kit/versiones/v1_area_conocimiento/4_research.md) | Las decisiones con la alternativa que se descartó |
| [5_data_model.md](docs/spec_kit/versiones/v1_area_conocimiento/5_data_model.md) | La tabla, sus semillas exactas y quién no puede escribir qué |
| [6_contracts.md](docs/spec_kit/versiones/v1_area_conocimiento/6_contracts.md) | Los 7 endpoints con TODOS sus códigos de respuesta |
| [7_quickstart.md](docs/spec_kit/versiones/v1_area_conocimiento/7_quickstart.md) | El smoke test, comando por comando |
| [8_tasks.md](docs/spec_kit/versiones/v1_area_conocimiento/8_tasks.md) | Las fases de construcción, cada una con su compuerta |
| [9_checklist.md](docs/spec_kit/versiones/v1_area_conocimiento/9_checklist.md) | La lista con la que se revisa la spec **antes** de programar |
| [GUIA_IA1.md](docs/spec_kit/versiones/v1_area_conocimiento/GUIA_IA1.md) | Cómo reconstruir esta versión con ayuda de IA, con el prompt listo |

> 🤖 **¿Va a trabajar con IA?** Empiece por la
> [GUIA_IA1](docs/spec_kit/versiones/v1_area_conocimiento/GUIA_IA1.md): trae
> los dos caminos —chat web e IDE agéntico— con su prompt exacto, y la
> regla que más importa: **cuando la IA dice "asumo que…", hay que pararla.**

## 5. Cómo se construyó esto

El [PLAN_V1.md](PLAN_V1.md) documenta el proceso completo, paso a paso: qué
se copió del ejemplo del curso, qué se adaptó, qué se escribió de cero, y
los defectos que aparecieron por el camino —incluidos los del propio script
de base de datos que entrega el módulo—.

Vale la pena leerlo por una razón: **la especificación se escribió primero,
y el código se construyó con IA siguiéndola.** Las correcciones que hubo no
fueron parches al código: fueron huecos del plan, que se taparon en el
documento y no en el programa.
