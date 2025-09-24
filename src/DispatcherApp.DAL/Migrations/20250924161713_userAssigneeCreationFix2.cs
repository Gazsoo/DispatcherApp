using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispatcherApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class userAssigneeCreationFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentUser_Assignments_AssignmentId",
                table: "AssignmentUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssignmentUser",
                table: "AssignmentUser");

            migrationBuilder.RenameTable(
                name: "AssignmentUser",
                newName: "AssignmentUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssignmentUsers",
                table: "AssignmentUsers",
                columns: new[] { "AssignmentId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentUsers_Assignments_AssignmentId",
                table: "AssignmentUsers",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentUsers_Assignments_AssignmentId",
                table: "AssignmentUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssignmentUsers",
                table: "AssignmentUsers");

            migrationBuilder.RenameTable(
                name: "AssignmentUsers",
                newName: "AssignmentUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssignmentUser",
                table: "AssignmentUser",
                columns: new[] { "AssignmentId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentUser_Assignments_AssignmentId",
                table: "AssignmentUser",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
