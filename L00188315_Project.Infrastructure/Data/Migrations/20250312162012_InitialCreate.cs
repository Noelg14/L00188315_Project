using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L00188315_Project.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountSubType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Iban = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                }
            );

            migrationBuilder.CreateTable(
                name: "Balances",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BalanceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balances", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Balances_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransctionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditDebitIndicator = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: true
                    ),
                    BookingDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValueDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreditorAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebtorAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProprietaryBankTransactionCode = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: true
                    ),
                    ProprietaryBankTransactionIssuer = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: true
                    ),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionInformation = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: true
                    ),
                    UserComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransctionId);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId"
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Balances");

            migrationBuilder.DropTable(name: "Transactions");

            migrationBuilder.DropTable(name: "Accounts");
        }
    }
}
