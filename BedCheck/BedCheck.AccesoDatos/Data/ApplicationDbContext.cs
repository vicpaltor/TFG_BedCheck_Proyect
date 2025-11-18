using BedCheck.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BedCheck.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //Poner aqui todos los modelos que se vayan creando
        public DbSet<Habitacion> Habitacion { get; set; }
        public DbSet<Cama> Cama { get; set; }
        public DbSet<Enfermero> Enfermero { get; set; }
        public DbSet<Paciente> Paciente { get; set; }
        public DbSet<Operacion> Operacion { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
