using BedCheck.AccesoDatos.Data.Repository;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BedCheck.AccesoDatos.Data;
using BedCheck.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL") ?? throw new InvalidOperationException("Connection string 'ConexionSQL' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();
builder.Services.AddControllersWithViews();

//Agregar contenedor de trabajo al contenedor IoC de inyeccion de dependencia
builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthentication(); // Asegúrate de que esta línea esté presente para la autenticación.

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{area=Identity}/{controller=Account}/{action=Login}/{id?}"); // Redirige al login como acción por defecto.

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Empleado}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
