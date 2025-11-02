using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispatcherApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Sessions4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "DispatcherSessions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "DispatcherSessions");
        }
    }
}
