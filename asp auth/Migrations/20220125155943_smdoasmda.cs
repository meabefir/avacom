using Microsoft.EntityFrameworkCore.Migrations;

namespace asp_auth.Migrations
{
    public partial class smdoasmda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BodyId",
                table: "Avatars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BrowsId",
                table: "Avatars",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyId",
                table: "Avatars");

            migrationBuilder.DropColumn(
                name: "BrowsId",
                table: "Avatars");
        }
    }
}
