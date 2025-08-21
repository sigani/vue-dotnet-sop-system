﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NCCSOP.Server.Migrations
{
    /// <inheritdoc />
    public partial class nullableCategoryIdSOP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SOPs_Categories_CategoryId",
                table: "SOPs");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "SOPs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SOPs_Categories_CategoryId",
                table: "SOPs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SOPs_Categories_CategoryId",
                table: "SOPs");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "SOPs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SOPs_Categories_CategoryId",
                table: "SOPs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
