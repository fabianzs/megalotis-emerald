using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP.NET_Core_Webapp.Migrations
{
    public partial class ProductionMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Reviews",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Reviews",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
