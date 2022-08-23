using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class IMForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Blocks_BlockHash",
                table: "Transactions",
                column: "BlockHash",
                principalTable: "Blocks",
                principalColumn: "Hash",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Blocks_BlockHash",
                table: "Transactions");
        }
    }
}
