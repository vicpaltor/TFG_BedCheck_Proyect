using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedCheck.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreacionTablaCama : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CamasOcupadas",
                table: "Habitacion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Cama",
                columns: table => new
                {
                    IdCama = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CamaUsada = table.Column<bool>(type: "bit", nullable: false),
                    EstadoCama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Operacion = table.Column<int>(type: "int", nullable: false),
                    TipoCama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlImagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HabitacionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cama", x => x.IdCama);
                    table.ForeignKey(
                        name: "FK_Cama_Habitacion_HabitacionId",
                        column: x => x.HabitacionId,
                        principalTable: "Habitacion",
                        principalColumn: "IdHabitacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cama_HabitacionId",
                table: "Cama",
                column: "HabitacionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cama");

            migrationBuilder.AlterColumn<int>(
                name: "CamasOcupadas",
                table: "Habitacion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
