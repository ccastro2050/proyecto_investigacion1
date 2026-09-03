using System.Net.Http.Json;
using System.Text.Json;

namespace FrontInvestigacion.Servicios;

/// <summary>
/// El área de conocimiento, tal como el front la maneja.
///
/// **Es una clase del front, no de la API.** Se parece a la de allá porque el
/// contrato es el mismo, y aun así son dos clases distintas en dos proyectos
/// distintos: si compartieran una biblioteca, los dos procesos dejarían de ser
/// independientes y el front podría romperse por un cambio interno de la API.
///
/// Lo único que los une es el JSON.
/// </summary>
public class AreaConocimiento
{
    public string Id { get; set; } = string.Empty;
    public string GranArea { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Disciplina { get; set; } = string.Empty;
}

/// <summary>
/// Lo que devuelve cada operación: si salió bien, qué trajo, y qué errores hay
/// que mostrar.
///
/// Existe para que las páginas **no vean códigos de estado**. Una página
/// pregunta «¿salió bien?», no «¿fue 200 o 204?».
/// </summary>
public record Resultado<T>(bool Ok, T? Datos, List<string> Errores)
{
    public static Resultado<T> Bien(T datos) => new(true, datos, new());
    public static Resultado<T> Mal(List<string> errores) => new(false, default, errores);
}

/// <summary>
/// ==========================================================================
/// LA CAPA DE DATOS DEL FRONT — y por qué es de `area_conocimiento` y no «de
/// cualquier tabla»
/// ==========================================================================
///
/// Este servicio es al front lo que el repositorio es a la API: la ÚNICA pieza
/// que sabe dónde viven los datos —en la API, nunca en la base de datos— y la
/// única que habla HTTP.
///
/// **Y es específico de un recurso, no genérico.** Podría escribirse un
/// `ApiService.Listar("area_conocimiento")` que sirviera para cualquier tabla,
/// y sería más corto. No se hace, por el Artículo 10.1 y por lo mismo que del
/// lado de la API: un método `Listar(string tabla)` no le dice a nadie qué
/// recursos existen, y el compilador deja de revisar si esa tabla es una de
/// las que hay.
///
/// Cuando el proyecto tenga ocho recursos habrá ocho servicios como este. Se
/// van a parecer mucho — y cada uno va a decir sus campos, sus mensajes y sus
/// operaciones, que es justamente lo que un molde único borra.
///
/// ==========================================================================
/// LO QUE ESTE ARCHIVO NO SABE, Y NO LE HACE FALTA
/// ==========================================================================
///
/// No sabe que la API está en C#. Da la casualidad de que sí lo está —el front
/// también— pero en ninguna línea se aprovecha: todo viaja como JSON por HTTP,
/// igual que si la API estuviera en Python.
///
/// Y no sabe que detrás hay SQL Server. Eso es asunto de la API.
/// </summary>
public class ServicioAreaConocimiento
{
    private readonly HttpClient _http;

    private static readonly JsonSerializerOptions _opciones = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly List<string> NoDisponible = new()
    {
        "El servicio no está disponible. ¿Está arriba la API?"
    };

    public ServicioAreaConocimiento(HttpClient http)
    {
        _http = http;
    }

    // ------------------------------------------------------------------
    // RF1 — Listar
    // ------------------------------------------------------------------
    public async Task<Resultado<List<AreaConocimiento>>> Listar(int limite = 1000)
    {
        try
        {
            var r = await _http.GetAsync($"/api/area_conocimiento?limite={limite}");

            // 204 es «no hay ninguna», y NO es un error: la pantalla muestra
            // un recuadro que lo dice, no un aviso rojo.
            if (r.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return Resultado<List<AreaConocimiento>>.Bien(new());
            }

            if (!r.IsSuccessStatusCode)
            {
                return Resultado<List<AreaConocimiento>>.Mal(await Mensajes(r));
            }

            // El sobre del contrato: { tabla, limite, total, datos[] }
            var sobre = await r.Content.ReadFromJsonAsync<JsonElement>();
            var datos = sobre.GetProperty("datos")
                .Deserialize<List<AreaConocimiento>>(_opciones) ?? new();

            return Resultado<List<AreaConocimiento>>.Bien(datos);
        }
        catch (HttpRequestException)
        {
            return Resultado<List<AreaConocimiento>>.Mal(NoDisponible);
        }
        catch (TaskCanceledException)
        {
            return Resultado<List<AreaConocimiento>>.Mal(NoDisponible);
        }
    }

    // ------------------------------------------------------------------
    // RF2 — Obtener una
    // ------------------------------------------------------------------
    public async Task<Resultado<AreaConocimiento>> Obtener(string id)
    {
        try
        {
            var r = await _http.GetAsync($"/api/area_conocimiento/{id}");
            if (!r.IsSuccessStatusCode)
            {
                return Resultado<AreaConocimiento>.Mal(await Mensajes(r));
            }

            var ficha = await r.Content.ReadFromJsonAsync<AreaConocimiento>(_opciones);
            return Resultado<AreaConocimiento>.Bien(ficha!);
        }
        catch (Exception e) when (e is HttpRequestException or TaskCanceledException)
        {
            return Resultado<AreaConocimiento>.Mal(NoDisponible);
        }
    }

    // ------------------------------------------------------------------
    // RF3 — Crear
    // ------------------------------------------------------------------
    public async Task<Resultado<bool>> Crear(AreaConocimiento area)
    {
        return await Enviar(HttpMethod.Post, "/api/area_conocimiento", area);
    }

    // ------------------------------------------------------------------
    // RF4 — Reemplazar: «guardar la ficha completa»
    //
    // El código NO va en el cuerpo: identifica la fila y viaja en la ruta.
    // ------------------------------------------------------------------
    public async Task<Resultado<bool>> Reemplazar(string id, AreaConocimiento area)
    {
        var cuerpo = new
        {
            granArea = area.GranArea,
            area = area.Area,
            disciplina = area.Disciplina
        };
        return await Enviar(HttpMethod.Put, $"/api/area_conocimiento/{id}", cuerpo);
    }

    // ------------------------------------------------------------------
    // RF5 — Actualizar: «guardar solo lo que cambié»
    //
    // Solo viaja lo diligenciado. Un campo en blanco NO se envía — no es que
    // se envíe vacío: sencillamente no va, y la API deja ese campo como estaba.
    // ------------------------------------------------------------------
    public async Task<Resultado<bool>> Actualizar(
        string id, string? granArea, string? area, string? disciplina)
    {
        var cuerpo = new Dictionary<string, string>();
        if (!string.IsNullOrWhiteSpace(granArea)) cuerpo["granArea"] = granArea;
        if (!string.IsNullOrWhiteSpace(area)) cuerpo["area"] = area;
        if (!string.IsNullOrWhiteSpace(disciplina)) cuerpo["disciplina"] = disciplina;

        return await Enviar(HttpMethod.Patch, $"/api/area_conocimiento/{id}", cuerpo);
    }

    // ------------------------------------------------------------------
    // RF6 — Retirar del uso (la API lo hace lógico: la fila no se borra)
    // ------------------------------------------------------------------
    public async Task<Resultado<bool>> Eliminar(string id)
    {
        return await Enviar(HttpMethod.Delete, $"/api/area_conocimiento/{id}", null);
    }

    // ------------------------------------------------------------------
    // Lo común a las cuatro operaciones que escriben
    // ------------------------------------------------------------------
    private async Task<Resultado<bool>> Enviar(HttpMethod metodo, string ruta, object? cuerpo)
    {
        try
        {
            var peticion = new HttpRequestMessage(metodo, ruta);
            if (cuerpo != null)
            {
                peticion.Content = JsonContent.Create(cuerpo);
            }

            var r = await _http.SendAsync(peticion);
            return r.IsSuccessStatusCode
                ? Resultado<bool>.Bien(true)
                : Resultado<bool>.Mal(await Mensajes(r));
        }
        catch (Exception e) when (e is HttpRequestException or TaskCanceledException)
        {
            return Resultado<bool>.Mal(NoDisponible);
        }
    }

    /// <summary>
    /// Traduce a texto los errores que produce ESTA API.
    ///
    /// El sobre es plano y tiene dos formas:
    ///   { estado, mensaje, detalle }   → 400, 404, 500
    ///   { estado, mensaje, errores[] } → 422, cuando el cuerpo no cumple
    ///
    /// **Este método es el único sitio del front que conoce ese formato.** Si
    /// mañana la API cambia el sobre, se cambia aquí y en ninguna página.
    /// </summary>
    private static async Task<List<string>> Mensajes(HttpResponseMessage r)
    {
        try
        {
            var sobre = await r.Content.ReadFromJsonAsync<JsonElement>();

            if (sobre.TryGetProperty("errores", out var errores)
                && errores.ValueKind == JsonValueKind.Array
                && errores.GetArrayLength() > 0)
            {
                return errores.EnumerateArray()
                    .Select(x => x.GetString() ?? "")
                    .Where(x => x.Length > 0)
                    .ToList();
            }

            var partes = new List<string>();
            if (sobre.TryGetProperty("mensaje", out var m)) partes.Add(m.GetString() ?? "");
            if (sobre.TryGetProperty("detalle", out var d)) partes.Add(d.GetString() ?? "");
            partes.RemoveAll(string.IsNullOrWhiteSpace);

            return partes.Count > 0
                ? partes
                : new List<string> { "No se pudo completar la operación." };
        }
        catch
        {
            // Un 500 puede devolver HTML en vez de JSON.
            return new List<string> { "No se pudo completar la operación." };
        }
    }
}
