using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispatcherApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Sessions3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Tutorials");

            migrationBuilder.AddColumn<int>(
                name: "PictureId",
                table: "Tutorials",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Assignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Log",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedTime",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DispatcherSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatcherSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispatcherSessions_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DispatcherSessions_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Tutorials_PictureId",
                table: "Tutorials",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatcherSessionIdentityUser_ParticipantsId",
                table: "DispatcherSessionIdentityUser",
                column: "ParticipantsId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatcherSessions_AssignmentId",
                table: "DispatcherSessions",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatcherSessions_OwnerId",
                table: "DispatcherSessions",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tutorials_Files_PictureId",
                table: "Tutorials",
                column: "PictureId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutorials_Files_PictureId",
                table: "Tutorials");

            migrationBuilder.DropTable(
                name: "DispatcherSessionIdentityUser");

            migrationBuilder.DropTable(
                name: "DispatcherSessions");

            migrationBuilder.DropIndex(
                name: "IX_Tutorials_PictureId",
                table: "Tutorials");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "Tutorials");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Log",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "PlannedTime",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Assignments");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Tutorials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
