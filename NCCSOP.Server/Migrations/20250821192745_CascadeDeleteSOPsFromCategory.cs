using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NCCSOP.Server.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteSOPsFromCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SOPs_Categories_CategoryId",
                table: "SOPs");

            migrationBuilder.AddForeignKey(
                name: "FK_SOPs_Categories_CategoryId",
                table: "SOPs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SOPs_Categories_CategoryId",
                table: "SOPs");

            migrationBuilder.AddForeignKey(
                name: "FK_SOPs_Categories_CategoryId",
                table: "SOPs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
