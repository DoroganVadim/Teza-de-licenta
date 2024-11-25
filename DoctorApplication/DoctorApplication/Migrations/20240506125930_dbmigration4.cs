using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorApplication.Migrations
{
    /// <inheritdoc />
    public partial class dbmigration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "doctors");

            migrationBuilder.AddColumn<int>(
                name: "accountid",
                table: "doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_doctors_accountid",
                table: "doctors",
                column: "accountid");

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_accounts_accountid",
                table: "doctors",
                column: "accountid",
                principalTable: "accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_accounts_accountid",
                table: "doctors");

            migrationBuilder.DropIndex(
                name: "IX_doctors_accountid",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "accountid",
                table: "doctors");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
