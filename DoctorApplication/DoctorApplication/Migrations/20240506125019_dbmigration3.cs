using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorApplication.Migrations
{
    /// <inheritdoc />
    public partial class dbmigration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "appointmentNum",
                table: "doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "appointmentNum",
                table: "doctors");
        }
    }
}
