using System.ComponentModel.DataAnnotations;

namespace ApiInvestigacion.Peticiones;

/// <summary>
/// Modelo de entrada para el reemplazo completo de un área de conocimiento (PUT).
/// Exige los 3 campos de datos. Si falta alguno, la validación falla retornando 422.
/// No incluye el campo Id porque la entidad se identifica mediante la ruta (URL).
/// </summary>
public class AreaConocimientoReemplazo
{
    [Required(ErrorMessage = "El campo granArea es obligatorio.")]
    public string GranArea { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo area es obligatorio.")]
    public string Area { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo disciplina es obligatorio.")]
    public string Disciplina { get; set; } = string.Empty;
}
