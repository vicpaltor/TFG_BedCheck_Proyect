using BedCheck.AccesoDatos.Data.Repository;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Data;
using BedCheck.Middleware;
using BedCheck.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using BedCheck.Servicios.Implementacion;
using BedCheck.Servicios.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region Configuración de Serilog (Logging)
// ============================================================
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/bedcheck_log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
#endregion

#region Configuración de Base de Datos e Identity
// ============================================================
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL")
    ?? throw new InvalidOperationException("Connection string 'ConexionSQL' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();
#endregion

#region Servicios Web (MVC, AutoMapper, HealthChecks)
// ============================================================
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICamaService, CamaService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(BedCheck.Mapping.MappingConfig));

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

// Inyección de Dependencias (Repositorios)
builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();

// SEGURIDAD: RATE LIMITING (Protección contra ataques DDoS)
builder.Services.AddRateLimiter(options =>
{
    // Si se pasan del límite, devolvemos error 429 (Too Many Requests)
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Definimos una política global:
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            // Agrupamos por IP del usuario (para limitar por persona)
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonimo",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,       // Máximo 100 peticiones...
                Window = TimeSpan.FromMinutes(1), // ...cada minuto
                QueueLimit = 0           // No encolamos peticiones extra, las rechazamos
            }));
});

#endregion

#region Configuración de Swagger (Documentación API)
// ============================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

var app = builder.Build();

#region SEGURIDAD AVANZADA: CABECERAS HTTP
// ============================================================
app.Use(async (context, next) =>
{
    // 1. Evita que tu web se abra en un iframe (Protección contra Clickjacking)
    context.Response.Headers.Append("X-Frame-Options", "DENY");

    // 2. Evita que el navegador "adivine" el tipo de archivo (Protección MIME-Sniffing)
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

    // 3. Activa el filtro anti-XSS de los navegadores antiguos
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");

    // 4. Controla cuánta información se envía al salir de tu web hacia otra
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");

    // 5. (Opcional) Content Security Policy - Muy estricto, puede romper estilos si no se configura bien.
    // De momento lo dejamos comentado para no romper DataTables o Bootstrap.
    // context.Response.Headers.Append("Content-Security-Policy", "default-src 'self';");

    await next();
});
#endregion

#region Pipeline de Peticiones HTTP (Middleware)
// ============================================================

// Manejo de Excepciones
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    // Swagger solo visible en desarrollo
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Middleware personalizado para errores en producción
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRateLimiter();

app.UseRouting();

app.UseAuthorization();

// Mapeo de Rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Empleado}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Ruta de Salud
app.MapHealthChecks("/health");

#endregion

app.Run();