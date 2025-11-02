using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispatcherApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Sessions2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispatcherSessions_AspNetUsers_OwnerId",
                table: "DispatcherSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_DispatcherSessions_Assignments_AssignmentId",
                table: "DispatcherSessions");

            migrationBuilder.DropTable(
                name: "DispatcherSessionIdentityUser");

            migrationBuilder.DropIndex(
                name: "IX_DispatcherSessions_OwnerId",
                table: "DispatcherSessions");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "DispatcherSessions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AssignmentId",
                table: "DispatcherSessions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "DispatcherSessions",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "DispatcherSessions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "DispatcherSessions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "SessionParticipant",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionParticipant", x => new { x.SessionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SessionParticipant_DispatcherSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "DispatcherSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DispatcherSessions_Assignments_AssignmentId",
                table: "DispatcherSessions",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispatcherSessions_Assignments_AssignmentId",
                table: "DispatcherSessions");

            migrationBuilder.DropTable(
                name: "SessionParticipant");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "DispatcherSessions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DispatcherSessions");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "DispatcherSessions");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "DispatcherSessions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AssignmentId",
                table: "DispatcherSessions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DispatcherSessionIdentityUser",
                columns: table => new
                {
                    DispatcherSessionId = table.Column<int>(type: "int", nullable: false),
                    ParticipantsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatcherSessionIdentityUser", x => new { x.DispatcherSessionId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_DispatcherSessionIdentityUser_AspNetUsers_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DispatcherSessionIdentityUser_DispatcherSessions_DispatcherSessionId",
                        column: x => x.DispatcherSessionId,
                        principalTable: "DispatcherSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DispatcherSessions_OwnerId",
                table: "DispatcherSessions",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatcherSessionIdentityUser_ParticipantsId",
                table: "DispatcherSessionIdentityUser",
                column: "ParticipantsId");

            migrationBuilder.AddForeignKey(
                name: "FK_DispatcherSessions_AspNetUsers_OwnerId",
                table: "DispatcherSessions",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DispatcherSessions_Assignments_AssignmentId",
                table: "DispatcherSessions",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
