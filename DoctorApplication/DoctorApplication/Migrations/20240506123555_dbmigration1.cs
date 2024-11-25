using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorApplication.Migrations
{
    /// <inheritdoc />
    public partial class dbmigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<int>(type: "int", nullable: false),
                    verified = table.Column<bool>(type: "bit", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cultures",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cultures", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "doctors",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    surrname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    activityStatus = table.Column<bool>(type: "bit", nullable: false),
                    imageString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    verified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "doctorSpecialities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctorSpecialities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "logEvents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _event = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logEvents", x => x.id);
                    table.ForeignKey(
                        name: "FK_logEvents_accounts_userId",
                        column: x => x.userId,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resources",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cultureid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resources", x => x.id);
                    table.ForeignKey(
                        name: "FK_resources_cultures_cultureid",
                        column: x => x.cultureid,
                        principalTable: "cultures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    namePacient = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    surrnamePacient = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idDoctor = table.Column<int>(type: "int", nullable: false),
                    doctorid = table.Column<int>(type: "int", nullable: false),
                    tel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    appointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    appointmentTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    confirmedUser = table.Column<bool>(type: "bit", nullable: false),
                    confirmedDoctor = table.Column<bool>(type: "bit", nullable: false),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    emailUser = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointments", x => x.id);
                    table.ForeignKey(
                        name: "FK_appointments_doctors_doctorid",
                        column: x => x.doctorid,
                        principalTable: "doctors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorDoctorSpecialitie",
                columns: table => new
                {
                    doctorsid = table.Column<int>(type: "int", nullable: false),
                    specialitiesid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorDoctorSpecialitie", x => new { x.doctorsid, x.specialitiesid });
                    table.ForeignKey(
                        name: "FK_DoctorDoctorSpecialitie_doctorSpecialities_specialitiesid",
                        column: x => x.specialitiesid,
                        principalTable: "doctorSpecialities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorDoctorSpecialitie_doctors_doctorsid",
                        column: x => x.doctorsid,
                        principalTable: "doctors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_doctorid",
                table: "appointments",
                column: "doctorid");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorDoctorSpecialitie_specialitiesid",
                table: "DoctorDoctorSpecialitie",
                column: "specialitiesid");

            migrationBuilder.CreateIndex(
                name: "IX_logEvents_userId",
                table: "logEvents",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_resources_cultureid",
                table: "resources",
                column: "cultureid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "DoctorDoctorSpecialitie");

            migrationBuilder.DropTable(
                name: "logEvents");

            migrationBuilder.DropTable(
                name: "resources");

            migrationBuilder.DropTable(
                name: "doctorSpecialities");

            migrationBuilder.DropTable(
                name: "doctors");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "cultures");
        }
    }
}
