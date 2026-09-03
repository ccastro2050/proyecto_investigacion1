"""Prueba de humo del FRONT Blazor.

QUÉ COMPRUEBA Y QUÉ NO — hay que decirlo, porque la limitación es real.

Blazor Server hace las interacciones por una conexión persistente: los clics,
los formularios y los botones NO son peticiones HTTP que un guion pueda
enviar. Este guion, entonces, no puede llenar un formulario como sí lo hace el
del front en Flask.

Lo que sí comprueba, que es la mitad que importa para cerrar la versión:

  · que cada pantalla RESPONDE por su dirección propia;
  · que el HTML que llega **ya trae los datos de la API** — Blazor los pide en
    el servidor antes de mandar la página, así que si aparecen aquí es que el
    front habló con la API de verdad;
  · que la pantalla NO le habla al usuario en jerga;
  · y lo más importante: que **con la API apagada la pantalla sigue en pie**,
    con su aviso adentro. Eso es lo que demuestra que son dos procesos.

Lo que queda para una persona: llenar el formulario, usar los dos botones de
guardar y retirar una ficha. Está escrito paso a paso en 7_quickstart.md.
"""
import html
import json
import re
import subprocess
import time
import urllib.error
import urllib.request

FRONT = "http://localhost:8071"
API = "http://localhost:8070"
fallos = []


def ver(url):
    try:
        with urllib.request.urlopen(url, timeout=15) as r:
            return r.status, r.read().decode("utf-8", "replace")
    except urllib.error.HTTPError as e:
        return e.code, e.read().decode("utf-8", "replace")
    except Exception as e:
        return 0, str(e)


def visible(pagina: str) -> str:
    """El texto que el usuario VE: sin etiquetas y con las tildes de verdad.

    Comprobar sobre el HTML crudo da dos falsos positivos, y los dos pasaron:

      · «422» aparece dentro del hash de integridad de un archivo estático
        —`…04022743…`—, que no es algo que nadie lea;
      · el aviso «El servicio no está disponible» llega codificado como
        `est&#xE1;`, así que buscar la «á» literal no lo encuentra.

    Las dos veces el sistema estaba bien y el guion mal. La lección: **una
    prueba de pantalla comprueba lo que se ve, no el código fuente de la
    página.**
    """
    sin_script = re.sub(r"(?is)<(script|style)[^>]*>.*?</\1>", " ", pagina)
    sin_etiquetas = re.sub(r"<[^>]*>", " ", sin_script)
    return re.sub(r"\s+", " ", html.unescape(sin_etiquetas))


def revisar(nombre, condicion, detalle=""):
    print(f"{'[OK]    ' if condicion else '[FALLO] '}{nombre} {detalle[:140]}")
    if not condicion:
        fallos.append(nombre)


def esperar_api(segundos=180):
    """Espera a que la API responda antes de comprobar nada contra ella.

    Hace falta porque este mismo guion la apaga y la enciende en la sección 5,
    y porque el contenedor corre `dotnet watch`: encenderla no es lo mismo que
    estar lista. Sin esta espera, la corrida siguiente empieza con la API a
    medio arrancar y falla por un motivo que no tiene que ver con lo que se
    está probando.
    """
    for _ in range(segundos // 3):
        if ver(f"{API}/api/area_conocimiento?limite=1")[0] == 200:
            return True
        time.sleep(3)
    return False


if not esperar_api():
    print("La API no respondió. ¿Está levantado el sistema?")
    raise SystemExit(1)

print("=== 1. Las pantallas responden, cada una por su dirección ===")
for ruta, titulo in [("/", "Sistema de investigación"),
                     ("/areas-de-conocimiento", "Áreas de conocimiento")]:
    c, t = ver(f"{FRONT}{ruta}")
    revisar(f"{ruta:26s} responde y se titula «{titulo}»",
            c == 200 and titulo in visible(t))

print()
print("=== 2. El menú lleva a la pantalla, con una dirección de verdad ===")
c, t = ver(f"{FRONT}/")
revisar("el menú tiene el enlace", 'href="areas-de-conocimiento"' in t)
revisar("y NO hay ninguna dirección con el nombre de la tabla como parámetro",
        "{tabla}" not in t and "{catalogo}" not in t)

print()
print("=== 3. La pantalla trae los datos que dio la API ===")
c, api = ver(f"{API}/api/area_conocimiento?limite=5")
primeras = [d["granArea"] for d in json.loads(api)["datos"]] if c == 200 else []
c, t = ver(f"{FRONT}/areas-de-conocimiento")
revisar("la API responde", c == 200 and len(primeras) > 0, f"{len(primeras)} filas")
revisar("y esos mismos datos se ven en la pantalla",
        all(g.split()[0] in visible(t) for g in primeras[:3]))
revisar("la tabla trae sus cuatro columnas",
        all(x in visible(t) for x in ("Código", "Gran área", "Área", "Disciplina")))

print()
print("=== 4. Lo que la pantalla NO debe decirle al usuario ===")
JERGA = ["PUT", "PATCH", "DELETE", "422", "500", "area_conocimiento",
         "SQL Server", "Dapper", "endpoint"]
for ruta in ("/", "/areas-de-conocimiento"):
    c, t = ver(f"{FRONT}{ruta}")
    visto = [j for j in JERGA if j in visible(t)]
    revisar(f"{ruta:26s} sin jerga", not visto, str(visto))

print()
print("=== 5. LA PRUEBA DE LOS DOS PROCESOS: se apaga la API ===")
print("    (esto tarda unos segundos)")
subprocess.run(["docker", "compose", "stop", "api-investigacion"],
               capture_output=True, text=True)
time.sleep(3)

c, t = ver(f"{FRONT}/areas-de-conocimiento")
revisar("la pantalla SIGUE respondiendo con la API apagada", c == 200)
revisar("  y muestra el aviso dentro de la aplicación",
        "no está disponible" in visible(t))
revisar("  con su menú y su marco intactos",
        "Áreas de conocimiento" in visible(t) and "Investigación" in visible(t))
revisar("  y SIN datos: el front no puede llegar a la base por su cuenta",
        "Ciencias Naturales" not in visible(t))

subprocess.run(["docker", "compose", "start", "api-investigacion"],
               capture_output=True, text=True)
print("    API encendida otra vez; esperando a que responda…")
for _ in range(40):
    c, _t = ver(f"{API}/api/area_conocimiento?limite=1")
    if c == 200:
        break
    time.sleep(3)
c, t = ver(f"{FRONT}/areas-de-conocimiento")
revisar("y al volver la API, la pantalla vuelve a traer los datos",
        "Ciencias Naturales" in visible(t))

print()
if fallos:
    print(f"=== RESULTADO: {len(fallos)} FALLO(S) ===")
    for f in fallos:
        print("   -", f)
else:
    print("=== RESULTADO: TODO EN VERDE ===")
    print()
    print("Falta lo que un guion no puede hacer con Blazor Server: llenar el")
    print("formulario y usar los dos botones de guardar. Está en 7_quickstart.md")
    print("como recorrido a mano, y lo hace una persona.")
