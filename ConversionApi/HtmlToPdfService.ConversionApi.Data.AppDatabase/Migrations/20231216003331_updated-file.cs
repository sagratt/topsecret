using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HtmlToPdfService.ConversionApi.Data.AppDatabase.Migrations
{
    public partial class updatedfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Files",
                newName: "StoredFileName");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Files",
                newName: "StoredFileLocation");

            migrationBuilder.AddColumn<string>(
                name: "ConvertedFileLocation",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConvertedFileName",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConvertedFileLocation",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ConvertedFileName",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "StoredFileName",
                table: "Files",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "StoredFileLocation",
                table: "Files",
                newName: "Location");
        }
    }
}
