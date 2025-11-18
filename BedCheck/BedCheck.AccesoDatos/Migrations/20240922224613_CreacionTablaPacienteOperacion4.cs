using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedCheck.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreacionTablaPacienteOperacion4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HabitacionId",
                table: "Operacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Operacion_HabitacionId",
                table: "Operacion",
                column: "HabitacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operacion_Habitacion_HabitacionId",
                table: "Operacion",
                column: "HabitacionId",
                principalTable: "Habitacion",
                principalColumn: "IdHabitacion",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operacion_Habitacion_HabitacionId",
                table: "Operacion");

            migrationBuilder.DropIndex(
                name: "IX_Operacion_HabitacionId",
                table: "Operacion");

            migrationBuilder.DropColumn(
                name: "HabitacionId",
                table: "Operacion");
        }
    }
}
