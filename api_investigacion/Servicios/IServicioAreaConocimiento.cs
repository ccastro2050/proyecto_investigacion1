using ApiInvestigacion.Modelos;

namespace ApiInvestigacion.Servicios;

/// <summary>
/// Contrato de la capa de negocio para el catálogo de áreas de conocimiento.
/// Define las operaciones que el controlador HTTP puede invocar, aislando completamente
/// la lógica del protocolo web y del motor de persistencia (Artículo 3).
///
/// Solo conoce Modelos/: las clases de Peticiones/ pertenecen a la frontera HTTP y no
/// cruzan a esta capa. Es el controlador quien traduce la petición en entidad o en
/// parámetros sueltos (3_plan.md §4.7).
///
/// Los problemas se comunican con excepciones de negocio, que el controlador traduce:
///   ArgumentException      → 400
///   NoEncontradoExcepcion  → 404
/// </summary>
public interface IServicioAreaConocimiento
{
    /// <summary>Hasta 'limite' áreas activas. ArgumentException si limite &lt;= 0.</summary>
    Task<IEnumerable<AreaConocimiento>> ObtenerTodas(int limite);

    /// <summary>El área con ese código. NoEncontradoExcepcion si no existe o está inactiva.</summary>
    Task<AreaConocimiento> ObtenerPorCodigo(string id);

    /// <summary>Crea el área. El cuerpo ya fue validado por AreaConocimientoCrear.</summary>
    Task Crear(AreaConocimiento area);

    /// <summary>Reemplazo completo. NoEncontradoExcepcion si no existe · devuelve filas afectadas.</summary>
    Task<int> Reemplazar(string id, AreaConocimiento area);

    /// <summary>Escribe solo los campos enviados. ArgumentException si no llegó ninguno ·
    /// NoEncontradoExcepcion si no existe · devuelve filas afectadas.</summary>
    Task<int> ActualizarParcial(string id, string? granArea, string? area, string? disciplina);

    /// <summary>Borrado lógico. NoEncontradoExcepcion si no existe o ya estaba inactiva.</summary>
    Task<int> Eliminar(string id);
}
