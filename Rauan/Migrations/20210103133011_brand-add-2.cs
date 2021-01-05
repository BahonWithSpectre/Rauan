using Microsoft.EntityFrameworkCore.Migrations;

namespace Rauan.Migrations
{
    public partial class brandadd2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Pod_Categories_Pod_CategoryId",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Brands_Pod_CategoryId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Pod_CategoryId",
                table: "Brands");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Brands",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Brands");

            migrationBuilder.AddColumn<int>(
                name: "Pod_CategoryId",
                table: "Brands",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Pod_CategoryId",
                table: "Brands",
                column: "Pod_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Pod_Categories_Pod_CategoryId",
                table: "Brands",
                column: "Pod_CategoryId",
                principalTable: "Pod_Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
