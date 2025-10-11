using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispatcherApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FilesModelMore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UploadedByUserId",
                table: "Files",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Files_UploadedByUserId",
                table: "Files",
                column: "UploadedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_AspNetUsers_UploadedByUserId",
                table: "Files",
                column: "UploadedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_AspNetUsers_UploadedByUserId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_UploadedByUserId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Files");

            migrationBuilder.AlterColumn<string>(
                name: "UploadedByUserId",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
