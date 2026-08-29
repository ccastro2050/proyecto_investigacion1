using ApiInvestigacion.Modelos;

namespace ApiInvestigacion.Repositorios;

/// <summary>
/// Contrato de persistencia para la entidad AreaConocimiento.
/// Permite aislar la capa de negocio (servicio) de la implementación técnica de la base de datos (Artículo 3).
/// </summary>
public interface IRepositorioAreaConocimiento
{
    Task<IEnumerable<AreaConocimiento>> ObtenerTodos(int limite);
    Task<AreaConocimiento?> ObtenerPorId(string id);
    Task<bool> Crear(AreaConocimiento area);
    Task<int> Reemplazar(AreaConocimiento area);
    Task<int> ActualizarParcial(string id, string? granArea, string? area, string? disciplina);
    Task<int> EliminarLogico(string id);
}
