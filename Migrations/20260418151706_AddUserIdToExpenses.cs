using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourWallet.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToExpenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_AppUserId",
                table: "Expenses",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_AppUserId",
                table: "Expenses",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_AppUserId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_AppUserId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Expenses");
        }
    }
}
