using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rauan.Migrations
{
    public partial class brandadd3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrandPodCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Pod_CategoryId = table.Column<int>(nullable: false),
                    BrandId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandPodCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandPodCategories_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandPodCategories_Pod_Categories_Pod_CategoryId",
                        column: x => x.Pod_CategoryId,
                        principalTable: "Pod_Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandPodCategories_BrandId",
                table: "BrandPodCategories",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandPodCategories_Pod_CategoryId",
                table: "BrandPodCategories",
                column: "Pod_CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandPodCategories");
        }
    }
}
