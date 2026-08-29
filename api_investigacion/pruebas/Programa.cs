using ApiInvestigacion.Excepciones;
using ApiInvestigacion.Modelos;
using ApiInvestigacion.Repositorios;
using ApiInvestigacion.Servicios;

namespace ApiInvestigacion.Pruebas;

/// <summary>
/// Implementación de un repositorio en memoria (falso) para la verificación de desacoplamiento (Criterio 7).
/// Permite probar la capa de servicio sin depender de una base de datos ni de la red.
/// </summary>
public class RepositorioFalso : IRepositorioAreaConocimiento
{
    private readonly List<AreaConocimiento> _baseDeDatosFalsa = new()
    {
        new AreaConocimiento { Id = "1A01", GranArea = "Ciencias Naturales", Area = "Matemáticas", Disciplina = "Matemáticas puras" }
    };

    public Task<IEnumerable<AreaConocimiento>> ObtenerTodos(int limite)
    {
        return Task.FromResult(_baseDeDatosFalsa.Take(limite).AsEnumerable());
    }

    public Task<AreaConocimiento?> ObtenerPorId(string id)
    {
        var elemento = _baseDeDatosFalsa.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(elemento);
    }

    public Task<bool> Crear(AreaConocimiento area)
    {
        _baseDeDatosFalsa.Add(area);
        return Task.FromResult(true);
    }

    public Task<int> Reemplazar(AreaConocimiento area)
    {
        var index = _baseDeDatosFalsa.FindIndex(x => x.Id == area.Id);
        if (index == -1) return Task.FromResult(0);
        _baseDeDatosFalsa[index] = area;
        return Task.FromResult(1);
    }

    public Task<int> ActualizarParcial(string id, string? granArea, string? area, string? disciplina)
    {
        var item = _baseDeDatosFalsa.FirstOrDefault(x => x.Id == id);
        if (item == null) return Task.FromResult(0);

        if (granArea != null) item.GranArea = granArea;
        if (area != null) item.Area = area;
        if (disciplina != null) item.Disciplina = disciplina;

        return Task.FromResult(1);
    }

    public Task<int> EliminarLogico(string id)
    {
        // Sacarlo de la lista reproduce el EFECTO OBSERVABLE del borrado lógico: deja de
        // listarse y deja de encontrarse. La entidad no tiene columna Activo —es un detalle
        // del motor, no del modelo— así que aquí no hay nada que marcar.
        var item = _baseDeDatosFalsa.FirstOrDefault(x => x.Id == id);
        if (item == null) return Task.FromResult(0);
        _baseDeDatosFalsa.Remove(item);
        return Task.FromResult(1);
    }
}

public class Programa
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("=== Ejecutando Prueba de Capas sin Base de Datos ===");

        // Inyección manual del repositorio falso en el servicio
        IRepositorioAreaConocimiento repoFalso = new RepositorioFalso();
        IServicioAreaConocimiento servicio = new ServicioAreaConocimiento(repoFalso);

        // Prueba 1: Obtener existente
        var existente = await servicio.ObtenerPorCodigo("1A01");
        Console.WriteLine($"[OK] Área obtenida correctamente: {existente.Disciplina}");

        // Prueba 2: Obtener inexistente lanza NoEncontradoExcepcion
        try
        {
            await servicio.ObtenerPorCodigo("9Z99");
            Console.WriteLine("[ERROR] Debió lanzar NoEncontradoExcepcion.");
        }
        catch (NoEncontradoExcepcion)
        {
            Console.WriteLine("[OK] Excepción NoEncontradoExcepcion capturada correctamente al buscar inexistente.");
        }

        // Prueba 3: Validación de limite invalido lanza ArgumentException
        try
        {
            await servicio.ObtenerTodas(0);
            Console.WriteLine("[ERROR] Debió lanzar ArgumentException.");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("[OK] Excepción ArgumentException capturada correctamente ante límite <= 0.");
        }

        // Prueba 4: PATCH sin ningún campo lanza ArgumentException, no NoEncontradoExcepcion.
        // Es la diferencia entre responder 400 y responder 404 (contrato §6).
        try
        {
            await servicio.ActualizarParcial("1A01", null, null, null);
            Console.WriteLine("[ERROR] Debió lanzar ArgumentException por cuerpo vacío.");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("[OK] Cuerpo vacío en actualización parcial rechazado con ArgumentException.");
        }

        // Prueba 5: eliminar dos veces. La segunda debe fallar como inexistente (C5).
        await servicio.Eliminar("1A01");
        Console.WriteLine("[OK] Primera eliminación realizada.");
        try
        {
            await servicio.Eliminar("1A01");
            Console.WriteLine("[ERROR] La segunda eliminación debió lanzar NoEncontradoExcepcion.");
        }
        catch (NoEncontradoExcepcion)
        {
            Console.WriteLine("[OK] Segunda eliminación rechazada: para la API ya no existe.");
        }

        Console.WriteLine("=== Prueba de capas completada CON ÉXITO ===");
    }
}
