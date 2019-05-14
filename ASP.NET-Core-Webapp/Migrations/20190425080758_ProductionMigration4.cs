using Microsoft.EntityFrameworkCore.Migrations;

namespace ASP.NET_Core_Webapp.Migrations
{
    public partial class ProductionMigration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "Users",
                newName: "Picture");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Picture",
                table: "Users",
                newName: "MyProperty");
        }
    }
}
