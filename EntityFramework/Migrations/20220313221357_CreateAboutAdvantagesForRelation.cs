using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFramework.Migrations
{
    public partial class CreateAboutAdvantagesForRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Advantages",
                table: "Abouts");

            migrationBuilder.CreateTable(
                name: "aboutAdvantages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Advantage = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    AboutId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aboutAdvantages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aboutAdvantages_Abouts_AboutId",
                        column: x => x.AboutId,
                        principalTable: "Abouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_aboutAdvantages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_aboutAdvantages_AboutId",
                table: "aboutAdvantages",
                column: "AboutId");

            migrationBuilder.CreateIndex(
                name: "IX_aboutAdvantages_ProductId",
                table: "aboutAdvantages",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aboutAdvantages");

            migrationBuilder.AddColumn<string>(
                name: "Advantages",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
