using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class IM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    Hash = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Hash);
                });

            migrationBuilder.CreateTable(
                name: "TransactionInputs",
                columns: table => new
                {
                    TnxHash = table.Column<string>(type: "text", nullable: false),
                    OutputIndex = table.Column<int>(type: "integer", nullable: false),
                    OutputTnx = table.Column<string>(type: "text", nullable: false),
                    InputWallet = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionInputs", x => new { x.TnxHash, x.OutputTnx, x.OutputIndex });
                });

            migrationBuilder.CreateTable(
                name: "TransactionOutputs",
                columns: table => new
                {
                    TnxHash = table.Column<string>(type: "text", nullable: false),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<long>(type: "bigint", nullable: false),
                    OutputWallet = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionOutputs", x => new { x.TnxHash, x.Index });
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TnxHash = table.Column<string>(type: "text", nullable: false),
                    BlockHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TnxHash);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Address = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Address);
                });

            migrationBuilder.InsertData(
                table: "Blocks",
                columns: new[] { "Hash", "Date" },
                values: new object[] { "coinbase", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "TransactionOutputs",
                columns: new[] { "Index", "TnxHash", "OutputWallet", "Value" },
                values: new object[] { -1, "coinbase", "coinbase", 0L });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TnxHash", "BlockHash" },
                values: new object[] { "coinbase", "coinbase" });

            migrationBuilder.InsertData(
                table: "Wallets",
                column: "Address",
                value: "coinbase");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionInputs_InputWallet",
                table: "TransactionInputs",
                column: "InputWallet");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionOutputs_OutputWallet",
                table: "TransactionOutputs",
                column: "OutputWallet");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BlockHash",
                table: "Transactions",
                column: "BlockHash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blocks");

            migrationBuilder.DropTable(
                name: "TransactionInputs");

            migrationBuilder.DropTable(
                name: "TransactionOutputs");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Wallets");
        }
    }
}
