namespace ApiInvestigacion.Excepciones;

/// <summary>
/// Excepción personalizada de la capa de negocio.
/// Permite al servicio comunicar que un recurso no fue encontrado o está inactivo
/// sin hacer referencia explícita al protocolo HTTP ni al código de estado 404 (Artículo 3 de la Constitución).
/// </summary>
public class NoEncontradoExcepcion : Exception
{
    public NoEncontradoExcepcion(string mensaje) : base(mensaje)
    {
    }
}
