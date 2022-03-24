using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ManyMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Produtor",
                table: "Cafe");

            migrationBuilder.DropColumn(
                name: "Regiao",
                table: "Cafe");

            migrationBuilder.AddColumn<int>(
                name: "ProdutorId",
                table: "Cafe",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegiaoId",
                table: "Cafe",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Produtor",
                columns: table => new
                {
                    ProdutorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdutorName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtor", x => x.ProdutorId);
                });

            migrationBuilder.CreateTable(
                name: "Regiao",
                columns: table => new
                {
                    RegiaoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegiaoName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regiao", x => x.RegiaoId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cafe_ProdutorId",
                table: "Cafe",
                column: "ProdutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cafe_RegiaoId",
                table: "Cafe",
                column: "RegiaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cafe_Produtor_ProdutorId",
                table: "Cafe",
                column: "ProdutorId",
                principalTable: "Produtor",
                principalColumn: "ProdutorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cafe_Regiao_RegiaoId",
                table: "Cafe",
                column: "RegiaoId",
                principalTable: "Regiao",
                principalColumn: "RegiaoId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cafe_Produtor_ProdutorId",
                table: "Cafe");

            migrationBuilder.DropForeignKey(
                name: "FK_Cafe_Regiao_RegiaoId",
                table: "Cafe");

            migrationBuilder.DropTable(
                name: "Produtor");

            migrationBuilder.DropTable(
                name: "Regiao");

            migrationBuilder.DropIndex(
                name: "IX_Cafe_ProdutorId",
                table: "Cafe");

            migrationBuilder.DropIndex(
                name: "IX_Cafe_RegiaoId",
                table: "Cafe");

            migrationBuilder.DropColumn(
                name: "ProdutorId",
                table: "Cafe");

            migrationBuilder.DropColumn(
                name: "RegiaoId",
                table: "Cafe");

            migrationBuilder.AddColumn<string>(
                name: "Produtor",
                table: "Cafe",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Regiao",
                table: "Cafe",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
