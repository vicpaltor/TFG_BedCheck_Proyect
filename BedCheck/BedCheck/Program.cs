using BedCheck.AccesoDatos.Data.Repository;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Data;
using BedCheck.Middleware;
using BedCheck.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region 1. Configuración de Serilog (Logging)
// ============================================================
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/bedcheck_log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
#endregion

#region 2. Configuración de Base de Datos e Identity
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

#region 3. Servicios Web (MVC, AutoMapper, HealthChecks)
// ============================================================
builder.Services.AddControllersWithViews();

// AutoMapper
builder.Services.AddAutoMapper(typeof(BedCheck.Mapping.MappingConfig));

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

// Inyección de Dependencias (Repositorios)
builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();
#endregion

#region 4. Configuración de Swagger (Documentación API)
// ============================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

var app = builder.Build();

#region 5. Pipeline de Peticiones HTTP (Middleware)
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