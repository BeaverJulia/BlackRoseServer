using Microsoft.EntityFrameworkCore.Migrations;

namespace BlackRose.Migrations
{
    public partial class changename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_picture",
                table: "picture");

            migrationBuilder.RenameTable(
                name: "picture",
                newName: "pictureContext");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pictureContext",
                table: "pictureContext",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_pictureContext",
                table: "pictureContext");

            migrationBuilder.RenameTable(
                name: "pictureContext",
                newName: "picture");

            migrationBuilder.AddPrimaryKey(
                name: "PK_picture",
                table: "picture",
                column: "Id");
        }
    }
}
