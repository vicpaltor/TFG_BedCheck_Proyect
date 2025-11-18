using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedCheck.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreacionTablaPacienteOperacion3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StrCamaAsignada",
                table: "Operacion");

            migrationBuilder.DropColumn(
                name: "StrHabitacionAsignada",
                table: "Operacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StrCamaAsignada",
                table: "Operacion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StrHabitacionAsignada",
                table: "Operacion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
