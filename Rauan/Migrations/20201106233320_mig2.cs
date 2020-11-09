using Microsoft.EntityFrameworkCore.Migrations;

namespace Rauan.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Info6",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Info7",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Info8",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Info6",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Info7",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Info8",
                table: "Products");
        }
    }
}
