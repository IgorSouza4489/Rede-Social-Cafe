using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CafeComment_Cafe_CafesId",
                table: "CafeComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CafeComment",
                table: "CafeComment");

            migrationBuilder.RenameTable(
                name: "CafeComment",
                newName: "CafeComments");

            migrationBuilder.RenameIndex(
                name: "IX_CafeComment_CafesId",
                table: "CafeComments",
                newName: "IX_CafeComments_CafesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CafeComments",
                table: "CafeComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CafeComments_Cafe_CafesId",
                table: "CafeComments",
                column: "CafesId",
                principalTable: "Cafe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CafeComments_Cafe_CafesId",
                table: "CafeComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CafeComments",
                table: "CafeComments");

            migrationBuilder.RenameTable(
                name: "CafeComments",
                newName: "CafeComment");

            migrationBuilder.RenameIndex(
                name: "IX_CafeComments_CafesId",
                table: "CafeComment",
                newName: "IX_CafeComment_CafesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CafeComment",
                table: "CafeComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CafeComment_Cafe_CafesId",
                table: "CafeComment",
                column: "CafesId",
                principalTable: "Cafe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
