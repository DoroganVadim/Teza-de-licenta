using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorIdToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_doctors_Doctor",
                table: "appointments");

            migrationBuilder.RenameColumn(
                name: "Doctor",
                table: "appointments",
                newName: "doctorId");

            migrationBuilder.RenameIndex(
                name: "IX_appointments_Doctor",
                table: "appointments",
                newName: "IX_appointments_doctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_doctors_doctorId",
                table: "appointments",
                column: "doctorId",
                principalTable: "doctors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_doctors_doctorId",
                table: "appointments");

            migrationBuilder.RenameColumn(
                name: "doctorId",
                table: "appointments",
                newName: "Doctor");

            migrationBuilder.RenameIndex(
                name: "IX_appointments_doctorId",
                table: "appointments",
                newName: "IX_appointments_Doctor");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_doctors_Doctor",
                table: "appointments",
                column: "Doctor",
                principalTable: "doctors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
