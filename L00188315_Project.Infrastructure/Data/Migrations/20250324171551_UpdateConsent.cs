using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace L00188315_Project.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConsent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConsentId",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ConsentId",
                table: "Accounts",
                column: "ConsentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Consents_ConsentId",
                table: "Accounts",
                column: "ConsentId",
                principalTable: "Consents",
                principalColumn: "ConsentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Consents_ConsentId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_ConsentId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ConsentId",
                table: "Accounts");
        }
    }
}
