using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OfxImports.Infra.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankAccount",
                columns: table => new
                {
                    BankAccountId = table.Column<Guid>(nullable: false),
                    type = table.Column<string>(nullable: true),
                    agencycode = table.Column<string>(nullable: true),
                    code = table.Column<int>(nullable: false),
                    accountcode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bankaccounts", x => x.BankAccountId);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(nullable: false),
                    bankaccountid = table.Column<Guid>(nullable: false),
                    type = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "(now() at time zone 'utc')"),
                    transactionvalue = table.Column<double>(nullable: false),
                    Description = table.Column<string>(unicode: false, maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_BankAccount",
                        column: x => x.bankaccountid,
                        principalTable: "BankAccount",
                        principalColumn: "BankAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_BankAccountId",
                table: "BankAccount",
                column: "BankAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_transactions_bankaccountid",
                table: "Transaction",
                column: "bankaccountid");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TransactionId",
                table: "Transaction",
                column: "TransactionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "BankAccount");
        }
    }
}
