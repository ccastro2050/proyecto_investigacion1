namespace ApiInvestigacion.Modelos;

/// <summary>
/// Entidad de dominio que representa la tabla area_conocimiento.
/// Se utiliza para transferir datos entre las capas de repositorio, servicio y controlador.
/// No incluye la propiedad 'Activo' porque es un detalle de implementación interno del borrado lógico
/// que no debe ser expuesto en las respuestas JSON de la API.
/// </summary>
public class AreaConocimiento
{
    /// <summary>
    /// Código alfanumérico primario (ej. '1A01'). Identifica de forma única el registro.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Categoría superior del catálogo (ej. 'Ciencias Naturales').
    /// </summary>
    public string GranArea { get; set; } = string.Empty;

    /// <summary>
    /// Subcategoría principal (ej. 'Matemáticas').
    /// </summary>
    public string Area { get; set; } = string.Empty;

    /// <summary>
    /// Especialidad o disciplina específica (ej. 'Matemáticas puras').
    /// </summary>
    public string Disciplina { get; set; } = string.Empty;
}
