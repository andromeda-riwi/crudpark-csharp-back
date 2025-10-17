using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CrudPark.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreateTicketsAndPagosTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mensualidades",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_cliente = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    correo_cliente = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    placa_vehiculo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    fecha_creacion = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    fecha_actualizacion = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensualidades", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Operadores",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    fecha_creacion = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operadores", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TarifasCatalogo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    valor_hora = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    valor_fraccion = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    tope_diario = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    tiempo_gracia_minutos = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarifasCatalogo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Placa = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    FechaIngreso = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FechaSalida = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Estado = table.Column<int>(type: "integer", nullable: false),
                    OperadorIngresoId = table.Column<long>(type: "bigint", nullable: false),
                    OperadorSalidaId = table.Column<long>(type: "bigint", nullable: true),
                    MensualidadId = table.Column<long>(type: "bigint", nullable: true),
                    MontoPagado = table.Column<decimal>(type: "numeric(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Mensualidades_MensualidadId",
                        column: x => x.MensualidadId,
                        principalTable: "Mensualidades",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Tickets_Operadores_OperadorIngresoId",
                        column: x => x.OperadorIngresoId,
                        principalTable: "Operadores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Operadores_OperadorSalidaId",
                        column: x => x.OperadorSalidaId,
                        principalTable: "Operadores",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracionSistema",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tarifa_activa_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionSistema", x => x.id);
                    table.ForeignKey(
                        name: "FK_ConfiguracionSistema_TarifasCatalogo_tarifa_activa_id",
                        column: x => x.tarifa_activa_id,
                        principalTable: "TarifasCatalogo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketId = table.Column<long>(type: "bigint", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    FechaPago = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Metodo = table.Column<int>(type: "integer", nullable: false),
                    OperadorId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagos_Operadores_OperadorId",
                        column: x => x.OperadorId,
                        principalTable: "Operadores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pagos_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionSistema_tarifa_activa_id",
                table: "ConfiguracionSistema",
                column: "tarifa_activa_id");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_OperadorId",
                table: "Pagos",
                column: "OperadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_TicketId",
                table: "Pagos",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_MensualidadId",
                table: "Tickets",
                column: "MensualidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_OperadorIngresoId",
                table: "Tickets",
                column: "OperadorIngresoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_OperadorSalidaId",
                table: "Tickets",
                column: "OperadorSalidaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracionSistema");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "TarifasCatalogo");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Mensualidades");

            migrationBuilder.DropTable(
                name: "Operadores");
        }
    }
}
