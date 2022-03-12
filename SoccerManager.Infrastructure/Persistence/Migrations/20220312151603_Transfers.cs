using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SoccerManager.Infrastructure.Persistence.Configurations.Migrations
{
    public partial class Transfers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransferItemId",
                table: "SoccerPlayers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SoccerPlayerId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoccerPlayers_TransferItemId",
                table: "SoccerPlayers",
                column: "TransferItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SoccerPlayers_Transfers_TransferItemId",
                table: "SoccerPlayers",
                column: "TransferItemId",
                principalTable: "Transfers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoccerPlayers_Transfers_TransferItemId",
                table: "SoccerPlayers");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_SoccerPlayers_TransferItemId",
                table: "SoccerPlayers");

            migrationBuilder.DropColumn(
                name: "TransferItemId",
                table: "SoccerPlayers");
        }
    }
}
