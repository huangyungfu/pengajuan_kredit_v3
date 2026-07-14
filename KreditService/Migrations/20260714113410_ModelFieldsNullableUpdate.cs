using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KreditService.Migrations
{
    /// <inheritdoc />
    public partial class ModelFieldsNullableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pengajuan_kredit",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    plafon = table.Column<decimal>(type: "numeric", nullable: false),
                    bunga = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    tenor = table.Column<int>(type: "integer", nullable: false),
                    angsuran = table.Column<decimal>(type: "numeric", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pengajuan_kredit", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_plafon",
                table: "pengajuan_kredit",
                column: "plafon");

            migrationBuilder.CreateIndex(
                name: "idx_tenor",
                table: "pengajuan_kredit",
                column: "tenor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pengajuan_kredit");
        }
    }
}
