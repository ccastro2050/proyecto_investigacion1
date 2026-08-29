namespace ApiInvestigacion.Peticiones;

/// <summary>
/// Modelo de entrada para la actualización parcial de un área de conocimiento (PATCH).
/// Declara todos los campos como opcionales (nullable) para permitir actualizar únicamente
/// los atributos enviados en la petición HTTP.
/// </summary>
public class AreaConocimientoActualizar
{
    public string? GranArea { get; set; }
    public string? Area { get; set; }
    public string? Disciplina { get; set; }
}
