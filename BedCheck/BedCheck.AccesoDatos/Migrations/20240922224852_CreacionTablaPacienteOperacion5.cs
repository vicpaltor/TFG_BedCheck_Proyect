using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedCheck.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreacionTablaPacienteOperacion5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operacion_Habitacion_HabitacionId",
                table: "Operacion");

            migrationBuilder.RenameColumn(
                name: "HabitacionId",
                table: "Operacion",
                newName: "CamaId");

            migrationBuilder.RenameIndex(
                name: "IX_Operacion_HabitacionId",
                table: "Operacion",
                newName: "IX_Operacion_CamaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operacion_Cama_CamaId",
                table: "Operacion",
                column: "CamaId",
                principalTable: "Cama",
                principalColumn: "IdCama",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operacion_Cama_CamaId",
                table: "Operacion");

            migrationBuilder.RenameColumn(
                name: "CamaId",
                table: "Operacion",
                newName: "HabitacionId");

            migrationBuilder.RenameIndex(
                name: "IX_Operacion_CamaId",
                table: "Operacion",
                newName: "IX_Operacion_HabitacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operacion_Habitacion_HabitacionId",
                table: "Operacion",
                column: "HabitacionId",
                principalTable: "Habitacion",
                principalColumn: "IdHabitacion",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
