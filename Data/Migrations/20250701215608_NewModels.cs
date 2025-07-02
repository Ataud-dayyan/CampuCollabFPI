using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class NewModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Groups",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Faculty",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SchoolLevel",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CreatedById",
                table: "Groups",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AspNetUsers_CreatedById",
                table: "Groups",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_CreatedById",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_CreatedById",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "SchoolLevel",
                table: "Groups");
        }
    }
}
