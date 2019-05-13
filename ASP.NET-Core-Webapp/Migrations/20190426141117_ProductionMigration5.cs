using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP.NET_Core_Webapp.Migrations
{
    public partial class ProductionMigration5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_BadgeLevels_BadgeLevelId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_BadgeLevelId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BadgeLevelId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "Pitches",
                newName: "Created");

            migrationBuilder.CreateTable(
                name: "UserLevels",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    BadgeLevelId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLevels", x => new { x.BadgeLevelId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserLevels_BadgeLevels_BadgeLevelId",
                        column: x => x.BadgeLevelId,
                        principalTable: "BadgeLevels",
                        principalColumn: "BadgeLevelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLevels_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLevels_UserId",
                table: "UserLevels",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLevels");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Pitches",
                newName: "TimeStamp");

            migrationBuilder.AddColumn<long>(
                name: "BadgeLevelId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_BadgeLevelId",
                table: "Users",
                column: "BadgeLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_BadgeLevels_BadgeLevelId",
                table: "Users",
                column: "BadgeLevelId",
                principalTable: "BadgeLevels",
                principalColumn: "BadgeLevelId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
