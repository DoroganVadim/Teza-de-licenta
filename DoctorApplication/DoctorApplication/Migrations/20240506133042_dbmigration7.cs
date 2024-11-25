using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorApplication.Migrations
{
    /// <inheritdoc />
    public partial class dbmigration7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_accounts_accountid",
                table: "doctors");

            migrationBuilder.AlterColumn<int>(
                name: "accountid",
                table: "doctors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_accounts_accountid",
                table: "doctors",
                column: "accountid",
                principalTable: "accounts",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_accounts_accountid",
                table: "doctors");

            migrationBuilder.AlterColumn<int>(
                name: "accountid",
                table: "doctors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_accounts_accountid",
                table: "doctors",
                column: "accountid",
                principalTable: "accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
