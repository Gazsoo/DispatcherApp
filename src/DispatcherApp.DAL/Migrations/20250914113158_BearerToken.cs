using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispatcherApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class BearerToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_assignments",
                table: "assignments");

            migrationBuilder.RenameTable(
                name: "assignments",
                newName: "Assignments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments");

            migrationBuilder.RenameTable(
                name: "Assignments",
                newName: "assignments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_assignments",
                table: "assignments",
                column: "Id");
        }
    }
}
