using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispatcherApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddFileToSessionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LogFileId",
                table: "DispatcherSessions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionParticipant_UserId",
                table: "SessionParticipant",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatcherSessions_LogFileId",
                table: "DispatcherSessions",
                column: "LogFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_DispatcherSessions_Files_LogFileId",
                table: "DispatcherSessions",
                column: "LogFileId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionParticipant_AspNetUsers_UserId",
                table: "SessionParticipant",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispatcherSessions_Files_LogFileId",
                table: "DispatcherSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionParticipant_AspNetUsers_UserId",
                table: "SessionParticipant");

            migrationBuilder.DropIndex(
                name: "IX_SessionParticipant_UserId",
                table: "SessionParticipant");

            migrationBuilder.DropIndex(
                name: "IX_DispatcherSessions_LogFileId",
                table: "DispatcherSessions");

            migrationBuilder.DropColumn(
                name: "LogFileId",
                table: "DispatcherSessions");
        }
    }
}
