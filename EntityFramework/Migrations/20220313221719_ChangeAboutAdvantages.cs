using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFramework.Migrations
{
    public partial class ChangeAboutAdvantages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_aboutAdvantages_Products_ProductId",
                table: "aboutAdvantages");

            migrationBuilder.DropIndex(
                name: "IX_aboutAdvantages_ProductId",
                table: "aboutAdvantages");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "aboutAdvantages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "aboutAdvantages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_aboutAdvantages_ProductId",
                table: "aboutAdvantages",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_aboutAdvantages_Products_ProductId",
                table: "aboutAdvantages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
