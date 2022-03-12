using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SoccerManager.Infrastructure.Persistence.Migrations
{
    public partial class SoccerPlayersAndTeams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoccerTeamId",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SoccerTeams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Budget = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoccerTeams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoccerPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MarketValue = table.Column<int>(type: "integer", nullable: false),
                    SoccerTeamId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoccerPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoccerPlayers_SoccerTeams_SoccerTeamId",
                        column: x => x.SoccerTeamId,
                        principalTable: "SoccerTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SoccerTeamId",
                table: "AspNetUsers",
                column: "SoccerTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_SoccerPlayers_SoccerTeamId",
                table: "SoccerPlayers",
                column: "SoccerTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SoccerTeams_SoccerTeamId",
                table: "AspNetUsers",
                column: "SoccerTeamId",
                principalTable: "SoccerTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SoccerTeams_SoccerTeamId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SoccerPlayers");

            migrationBuilder.DropTable(
                name: "SoccerTeams");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SoccerTeamId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SoccerTeamId",
                table: "AspNetUsers");
        }
    }
}
