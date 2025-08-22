using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NCCSOP.Server.Migrations
{
    /// <inheritdoc />
    public partial class NameFieldRemoveEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "SOPItems");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SOPItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "SOPItems");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "SOPItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
