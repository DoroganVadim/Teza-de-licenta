using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorApplication.Migrations
{
    /// <inheritdoc />
    public partial class dbmigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_doctors_doctorid",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_doctorid",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "doctorid",
                table: "appointments");

            migrationBuilder.RenameColumn(
                name: "idDoctor",
                table: "appointments",
                newName: "Doctor");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_Doctor",
                table: "appointments",
                column: "Doctor");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_doctors_Doctor",
                table: "appointments",
                column: "Doctor",
                principalTable: "doctors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_doctors_Doctor",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_Doctor",
                table: "appointments");

            migrationBuilder.RenameColumn(
                name: "Doctor",
                table: "appointments",
                newName: "idDoctor");

            migrationBuilder.AddColumn<int>(
                name: "doctorid",
                table: "appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_appointments_doctorid",
                table: "appointments",
                column: "doctorid");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_doctors_doctorid",
                table: "appointments",
                column: "doctorid",
                principalTable: "doctors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
