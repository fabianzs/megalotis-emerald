using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP.NET_Core_Webapp.Migrations
{
    public partial class ProductionMigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    BadgeId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.BadgeId);
                });

            migrationBuilder.CreateTable(
                name: "BadgeLevels",
                columns: table => new
                {
                    BadgeLevelId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Level = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    BadgeId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgeLevels", x => x.BadgeLevelId);
                    table.ForeignKey(
                        name: "FK_BadgeLevels_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "BadgeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    MyProperty = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    OpenId = table.Column<string>(nullable: true),
                    BadgeLevelId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_BadgeLevels_BadgeLevelId",
                        column: x => x.BadgeLevelId,
                        principalTable: "BadgeLevels",
                        principalColumn: "BadgeLevelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pitches",
                columns: table => new
                {
                    PitchId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    PitchedLevel = table.Column<int>(nullable: false),
                    PitchMessage = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    BadgeId = table.Column<long>(nullable: true),
                    BadgeLevelId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pitches", x => x.PitchId);
                    table.ForeignKey(
                        name: "FK_Pitches_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "BadgeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pitches_BadgeLevels_BadgeLevelId",
                        column: x => x.BadgeLevelId,
                        principalTable: "BadgeLevels",
                        principalColumn: "BadgeLevelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pitches_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    PitchId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Pitches_PitchId",
                        column: x => x.PitchId,
                        principalTable: "Pitches",
                        principalColumn: "PitchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BadgeLevels_BadgeId",
                table: "BadgeLevels",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Pitches_BadgeId",
                table: "Pitches",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Pitches_BadgeLevelId",
                table: "Pitches",
                column: "BadgeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Pitches_UserId",
                table: "Pitches",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PitchId",
                table: "Reviews",
                column: "PitchId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BadgeLevelId",
                table: "Users",
                column: "BadgeLevelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Pitches");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "BadgeLevels");

            migrationBuilder.DropTable(
                name: "Badges");
        }
    }
}
