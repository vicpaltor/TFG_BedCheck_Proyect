using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedCheck.Data.Migrations
{
    /// <inheritdoc />
    public partial class CrearModeloHabitacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Habitacion",
                columns: table => new
                {
                    IdHabitacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CamasOcupadas = table.Column<int>(type: "int", nullable: false),
                    ListEnfermedadesTratamientos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumCamasTotales = table.Column<int>(type: "int", nullable: false),
                    NumHabitacion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habitacion", x => x.IdHabitacion);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Habitacion");
        }
    }
}
