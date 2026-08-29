using ApiInvestigacion.Excepciones;
using ApiInvestigacion.Modelos;
using ApiInvestigacion.Peticiones;
using ApiInvestigacion.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApiInvestigacion.Controllers;

/// <summary>
/// Controlador HTTP para el catálogo area_conocimiento (Capa 1).
/// No contiene lógica de negocio ni sentencias SQL (Artículo 3).
/// Se encarga exclusivamente de validar peticiones, delegar la ejecución al servicio e interpretar
/// excepciones para retornar las respuestas y códigos HTTP según el contrato (6_contracts.md).
///
/// La ruta se escribe COMPLETA y no con [controller]: el contrato exige /api/area_conocimiento,
/// con guion bajo, y el token generaría "AreaConocimiento" (Artículo 10).
///
/// Es también el traductor de la frontera: convierte las clases de Peticiones/ en la entidad o en
/// los campos sueltos que entiende la capa 2, que no conoce el cuerpo HTTP (3_plan.md §4.7).
/// </summary>
[ApiController]
[Route("api/area_conocimiento")]
public class AreaConocimientoController : ControllerBase
{
    private readonly IServicioAreaConocimiento _servicio;

    public AreaConocimientoController(IServicioAreaConocimiento servicio)
    {
        _servicio = servicio;
    }

    /// <summary>
    /// RF1 — Listar áreas de conocimiento activas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObtenerTodas([FromQuery] int limite = 1000)
    {
        try
        {
            var datos = await _servicio.ObtenerTodas(limite);
            var lista = datos.ToList();

            if (lista.Count == 0)
            {
                return NoContent(); // 204 sin cuerpo
            }

            return Ok(new
            {
                tabla = "area_conocimiento",
                limite = limite,
                total = lista.Count,
                datos = lista
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { estado = 400, mensaje = "Parámetros inválidos.", detalle = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { estado = 500, mensaje = "Error interno del servidor.", detalle = ex.Message });
        }
    }

    /// <summary>
    /// RF2 — Obtener área de conocimiento por su código primario.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(string id)
    {
        try
        {
            var area = await _servicio.ObtenerPorCodigo(id);
            return Ok(area);
        }
        catch (NoEncontradoExcepcion ex)
        {
            return NotFound(new { estado = 404, mensaje = "Área de conocimiento no encontrada.", detalle = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { estado = 500, mensaje = "Error interno del servidor.", detalle = ex.Message });
        }
    }

    /// <summary>
    /// RF3 — Crear un nuevo registro en el catálogo.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] AreaConocimientoCrear peticion)
    {
        try
        {
            // El controlador traduce: la capa 2 recibe la entidad, no el cuerpo HTTP
            var area = new AreaConocimiento
            {
                Id = peticion.Id,
                GranArea = peticion.GranArea,
                Area = peticion.Area,
                Disciplina = peticion.Disciplina
            };

            await _servicio.Crear(area);
            return Ok(new { estado = 200, mensaje = "Área de conocimiento creada exitosamente." });
        }
        catch (SqlException ex)
        {
            // Errores del motor como clave primaria duplicada retornan 500 (C8)
            return StatusCode(500, new { estado = 500, mensaje = "Error al insertar en la base de datos.", detalle = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { estado = 500, mensaje = "Error interno del servidor.", detalle = ex.Message });
        }
    }

    /// <summary>
    /// RF4 — Reemplazar completamente un registro existente.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Reemplazar(string id, [FromBody] AreaConocimientoReemplazo peticion)
    {
        try
        {
            var area = new AreaConocimiento
            {
                Id = id,
                GranArea = peticion.GranArea,
                Area = peticion.Area,
                Disciplina = peticion.Disciplina
            };

            var filas = await _servicio.Reemplazar(id, area);
            return Ok(new { estado = 200, mensaje = "Área de conocimiento reemplazada.", filasAfectadas = filas });
        }
        catch (NoEncontradoExcepcion ex)
        {
            return NotFound(new { estado = 404, mensaje = "Área de conocimiento no encontrada.", detalle = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { estado = 500, mensaje = "Error interno del servidor.", detalle = ex.Message });
        }
    }

    /// <summary>
    /// RF5 — Actualizar parcialmente los atributos de un registro.
    /// </summary>
    [HttpPatch("{id}")]
    public async Task<IActionResult> ActualizarParcial(string id, [FromBody] AreaConocimientoActualizar peticion)
    {
        try
        {
            var filas = await _servicio.ActualizarParcial(id, peticion.GranArea, peticion.Area, peticion.Disciplina);
            return Ok(new { estado = 200, mensaje = "Área de conocimiento actualizada.", filasAfectadas = filas });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { estado = 400, mensaje = "Parámetros inválidos.", detalle = ex.Message });
        }
        catch (NoEncontradoExcepcion ex)
        {
            return NotFound(new { estado = 404, mensaje = "Área de conocimiento no encontrada.", detalle = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { estado = 500, mensaje = "Error interno del servidor.", detalle = ex.Message });
        }
    }

    /// <summary>
    /// RF6 — Borrado lógico del registro.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(string id)
    {
        try
        {
            var filas = await _servicio.Eliminar(id);
            return Ok(new { estado = 200, mensaje = "Área de conocimiento eliminada.", filasAfectadas = filas });
        }
        catch (NoEncontradoExcepcion ex)
        {
            return NotFound(new { estado = 404, mensaje = "Área de conocimiento no encontrada.", detalle = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { estado = 500, mensaje = "Error interno del servidor.", detalle = ex.Message });
        }
    }
}
