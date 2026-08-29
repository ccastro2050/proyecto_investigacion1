using ApiInvestigacion.Excepciones;
using ApiInvestigacion.Modelos;
using ApiInvestigacion.Repositorios;

namespace ApiInvestigacion.Servicios;

/// <summary>
/// Implementación de la capa de servicio (reglas de negocio).
/// Depende exclusivamente de la interfaz IRepositorioAreaConocimiento, sin conocer detalles del motor (Artículo 3).
/// Traduce ausencias de datos en la excepción de dominio NoEncontradoExcepcion.
///
/// No conoce las clases de Peticiones/: recibe entidades y campos sueltos, porque la forma
/// del cuerpo HTTP es asunto del controlador (3_plan.md §4.7).
/// </summary>
public class ServicioAreaConocimiento : IServicioAreaConocimiento
{
    private readonly IRepositorioAreaConocimiento _repositorio;

    public ServicioAreaConocimiento(IRepositorioAreaConocimiento repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<AreaConocimiento>> ObtenerTodas(int limite)
    {
        // Regla de negocio: el límite debe ser un número entero estrictamente positivo (RF1 / C7)
        if (limite <= 0)
        {
            throw new ArgumentException("El parámetro limite debe ser un número mayor a 0.");
        }

        return await _repositorio.ObtenerTodos(limite);
    }

    public async Task<AreaConocimiento> ObtenerPorCodigo(string id)
    {
        var area = await _repositorio.ObtenerPorId(id);
        if (area == null)
        {
            throw new NoEncontradoExcepcion($"No existe un área de conocimiento con el código {id}.");
        }

        return area;
    }

    public async Task Crear(AreaConocimiento area)
    {
        await _repositorio.Crear(area);
    }

    public async Task<int> Reemplazar(string id, AreaConocimiento area)
    {
        // El código identifica la fila y viene de la ruta, no del cuerpo (D-v1-5)
        area.Id = id;

        var filasAfectadas = await _repositorio.Reemplazar(area);
        if (filasAfectadas == 0)
        {
            throw new NoEncontradoExcepcion($"No existe un área de conocimiento con el código {id}.");
        }

        return filasAfectadas;
    }

    public async Task<int> ActualizarParcial(string id, string? granArea, string? area, string? disciplina)
    {
        // Regla de negocio: PATCH debe recibir al menos un campo para actualizar (RF5).
        // Sin esta comprobación el repositorio devolvería 0 filas, que aquí significa
        // "no existe" y terminaría respondiendo 404 en vez del 400 que exige el contrato.
        if (granArea == null && area == null && disciplina == null)
        {
            throw new ArgumentException("No se envió ningún campo para actualizar.");
        }

        var filasAfectadas = await _repositorio.ActualizarParcial(id, granArea, area, disciplina);
        if (filasAfectadas == 0)
        {
            throw new NoEncontradoExcepcion($"No existe un área de conocimiento con el código {id}.");
        }

        return filasAfectadas;
    }

    public async Task<int> Eliminar(string id)
    {
        var filasAfectadas = await _repositorio.EliminarLogico(id);
        if (filasAfectadas == 0)
        {
            throw new NoEncontradoExcepcion($"No existe un área de conocimiento con el código {id}.");
        }

        return filasAfectadas;
    }
}
