using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class sauhdghuasdg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlFoto",
                table: "Cafe",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlFoto",
                table: "Cafe");
        }
    }
}
