using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L00188315_Project.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class FixBalances_Again : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Balances",
                table: "Balances");

            migrationBuilder.AddColumn<string>(
                name: "BalanceId",
                table: "Balances",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Balances",
                table: "Balances",
                column: "BalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Balances_AccountId",
                table: "Balances",
                column: "AccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Balances",
                table: "Balances");

            migrationBuilder.DropIndex(
                name: "IX_Balances_AccountId",
                table: "Balances");

            migrationBuilder.DropColumn(
                name: "BalanceId",
                table: "Balances");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Balances",
                table: "Balances",
                column: "AccountId");
        }
    }
}
