using System.ComponentModel.DataAnnotations;

namespace ApiInvestigacion.Peticiones;

/// <summary>
/// Modelo de entrada para la creación de un área de conocimiento (POST).
/// Requiere estrictamente los 4 campos del registro para garantizar la integridad inicial.
/// </summary>
public class AreaConocimientoCrear
{
    [Required(ErrorMessage = "El campo id es obligatorio.")]
    [MaxLength(6, ErrorMessage = "El campo id no puede exceder los 6 caracteres.")]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo granArea es obligatorio.")]
    public string GranArea { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo area es obligatorio.")]
    public string Area { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo disciplina es obligatorio.")]
    public string Disciplina { get; set; } = string.Empty;
}
