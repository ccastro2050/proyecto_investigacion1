using System.Data;
using ApiInvestigacion.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ApiInvestigacion.Repositorios;

/// <summary>
/// Implementación del repositorio para SQL Server utilizando Dapper (Artículo 2).
/// Escribe todas las consultas T-SQL a mano y garantiza que todos los parámetros estén
/// adecuadamente vinculados (@parametro) para evitar inyección de SQL.
/// Implementa estrictamente el filtro 'activo = 1' y el borrado lógico (Artículo 6).
/// </summary>
public class RepositorioAreaConocimientoSqlServer : IRepositorioAreaConocimiento
{
    private readonly string _cadenaConexion;

    public RepositorioAreaConocimientoSqlServer(IConfiguration configuracion)
    {
        // Se obtiene la cadena de conexión inyectada desde el ensamblador / variables de entorno
        _cadenaConexion = configuracion.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'SqlServer'.");
    }

    private IDbConnection CrearConexion() => new SqlConnection(_cadenaConexion);

    public async Task<IEnumerable<AreaConocimiento>> ObtenerTodos(int limite)
    {
        using var conexion = CrearConexion();
        // Consulta SQL explícita filtrando registros activos y aplicando límite estricto
        const string sql = @"
            SELECT TOP (@Limite) id AS Id, gran_area AS GranArea, area AS Area, disciplina AS Disciplina 
            FROM area_conocimiento 
            WHERE activo = 1 
            ORDER BY id ASC";

        return await conexion.QueryAsync<AreaConocimiento>(sql, new { Limite = limite });
    }

    public async Task<AreaConocimiento?> ObtenerPorId(string id)
    {
        using var conexion = CrearConexion();
        // Garantiza que registros inactivos (activo = 0) no sean retornados (C4)
        const string sql = @"
            SELECT id AS Id, gran_area AS GranArea, area AS Area, disciplina AS Disciplina 
            FROM area_conocimiento 
            WHERE id = @Id AND activo = 1";

        return await conexion.QueryFirstOrDefaultAsync<AreaConocimiento>(sql, new { Id = id });
    }

    public async Task<bool> Crear(AreaConocimiento area)
    {
        using var conexion = CrearConexion();
        // Inserta la entidad asignando por defecto activo = 1.
        const string sql = @"
            INSERT INTO area_conocimiento (id, gran_area, area, disciplina, activo) 
            VALUES (@Id, @GranArea, @Area, @Disciplina, 1)";

        var filasAfectadas = await conexion.ExecuteAsync(sql, area);
        return filasAfectadas > 0;
    }

    public async Task<int> Reemplazar(AreaConocimiento area)
    {
        using var conexion = CrearConexion();
        // Reemplazo completo de campos condicionado a que el registro exista y esté activo
        const string sql = @"
            UPDATE area_conocimiento 
            SET gran_area = @GranArea, area = @Area, disciplina = @Disciplina 
            WHERE id = @Id AND activo = 1";

        return await conexion.ExecuteAsync(sql, area);
    }

    public async Task<int> ActualizarParcial(string id, string? granArea, string? area, string? disciplina)
    {
        using var conexion = CrearConexion();

        // Se construye el SQL dinámico de actualización asegurando parametrización individual en cada campo.
        // OJO: lo que se compone son NOMBRES DE COLUMNA de una lista cerrada, nunca valores:
        // los valores siempre viajan como @parametro (3_plan.md §4.8).
        var asignaciones = new List<string>();
        var parametros = new DynamicParameters();
        parametros.Add("Id", id);

        if (granArea != null)
        {
            asignaciones.Add("gran_area = @GranArea");
            parametros.Add("GranArea", granArea);
        }
        if (area != null)
        {
            asignaciones.Add("area = @Area");
            parametros.Add("Area", area);
        }
        if (disciplina != null)
        {
            asignaciones.Add("disciplina = @Disciplina");
            parametros.Add("Disciplina", disciplina);
        }

        if (asignaciones.Count == 0) return 0;

        var sql = $"UPDATE area_conocimiento SET {string.Join(", ", asignaciones)} WHERE id = @Id AND activo = 1";

        return await conexion.ExecuteAsync(sql, parametros);
    }

    public async Task<int> EliminarLogico(string id)
    {
        using var conexion = CrearConexion();
        // Borrado lógico: se cambia activo = 0 en lugar de ejecutar DELETE (Artículo 6).
        // Devuelve 0 filas afectadas si el registro no existe o ya estaba inactivo.
        const string sql = @"
            UPDATE area_conocimiento 
            SET activo = 0 
            WHERE id = @Id AND activo = 1";

        return await conexion.ExecuteAsync(sql, new { Id = id });
    }
}
