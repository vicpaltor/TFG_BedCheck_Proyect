using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedCheck.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreacionTablaPacienteOperacion2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operacion",
                columns: table => new
                {
                    IdOperacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StrNombreOperacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StrEstadoOperacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StrFechaOperacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StrHabitacionAsignada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StrCamaAsignada = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operacion", x => x.IdOperacion);
                });

            migrationBuilder.CreateTable(
                name: "Paciente",
                columns: table => new
                {
                    IdPaciente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StrNombrePaciente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntEdadPaciente = table.Column<int>(type: "int", nullable: false),
                    StrSexoPaciente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListEnfermedades = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListTratamiento = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paciente", x => x.IdPaciente);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operacion");

            migrationBuilder.DropTable(
                name: "Paciente");
        }
    }
}
