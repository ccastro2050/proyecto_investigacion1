using ApiInvestigacion.Repositorios;
using ApiInvestigacion.Servicios;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// La API escucha en el 8070 también DENTRO del contenedor, para que el Dockerfile,
// el docker-compose y los contratos digan todos el mismo número (3_plan.md §5.2).
builder.WebHost.UseUrls("http://0.0.0.0:8070");

// ============================================================
// EL ENSAMBLADOR (Artículo 3)
// Estas dos líneas son el ÚNICO lugar del proyecto donde una clase concreta
// aparece junto a su interfaz. Todo lo demás recibe interfaces por constructor.
// ============================================================
builder.Services.AddScoped<IRepositorioAreaConocimiento, RepositorioAreaConocimientoSqlServer>();
builder.Services.AddScoped<IServicioAreaConocimiento, ServicioAreaConocimiento>();

builder.Services.AddControllers();

// ============================================================
// EL 422 DEL CONTRATO (3_plan.md §4.9)
// Con [ApiController], un cuerpo inválido corta la petición ANTES de entrar al
// método y responde 400 con ProblemDetails. El contrato exige 422 con el sobre
// {estado, mensaje, errores[]}: hay que reemplazar la fábrica de respuestas.
// ============================================================
builder.Services.Configure<ApiBehaviorOptions>(opciones =>
{
    opciones.InvalidModelStateResponseFactory = contexto =>
    {
        var errores = contexto.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .SelectMany(e => e.Value!.Errors.Select(x => x.ErrorMessage))
            .ToList();

        return new ObjectResult(new
        {
            estado = 422,
            mensaje = "Datos inválidos.",
            errores
        })
        { StatusCode = 422 };
    };
});

// Documentación interactiva
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// RF7 — Diagnóstico: dice quién es y qué versión, sin tocar la base de datos.
// Si esto no responde, el problema no es de contrato: la API no está arriba.
app.MapGet("/", () => Results.Ok(new
{
    mensaje = "API Investigación — módulo de áreas de conocimiento",
    version = "v1",
    contratos = "/swagger"
}));

app.MapControllers();

app.Run();
