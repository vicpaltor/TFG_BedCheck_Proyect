using BedCheck.AccesoDatos.Data.Repository;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Data;
using BedCheck.Middleware; // <--- ¡IMPORTANTE! Asegúrate de tener este using
using BedCheck.Models;
using BedCheck.Models.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog; // Importante para los logs

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DE SERILOG
// ==========================================
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/bedcheck_log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ==========================================
// Configuración de Controladores + FluentValidation
builder.Services.AddControllersWithViews();

builder.Services.AddFluentValidationAutoValidation(); // Valida automáticamente al recibir datos
builder.Services.AddFluentValidationClientsideAdapters(); // Agrega validación visual en el navegador (JS)
builder.Services.AddValidatorsFromAssemblyContaining<CamaValidator>(); // Registra todos los validadores que estén junto a CamaValidator
// ==========================================

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL")
    ?? throw new InvalidOperationException("Connection string 'ConexionSQL' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();

builder.Services.AddControllersWithViews();

// Inyección de dependencias
builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // En producción usamos esto, pero nuestro middleware actuará antes si hay error grave
    app.UseExceptionHandler("/Home/Error");
}

// 2. REGISTRAR EL MIDDLEWARE PERSONALIZADO
// ==========================================
// Esto capturará errores y los guardará en el Log antes de que la app se rompa
app.UseMiddleware<ExceptionHandlingMiddleware>();
// ==========================================

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Empleado}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();