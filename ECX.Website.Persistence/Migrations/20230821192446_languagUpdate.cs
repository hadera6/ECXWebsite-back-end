using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECX.Website.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class languagUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "ImgName",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "LangId",
                table: "Languages");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Accounts",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "LangId",
                table: "Accounts",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Accounts",
                newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Accounts",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Accounts",
                newName: "LangId");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Accounts",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Languages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImgName",
                table: "Languages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LangId",
                table: "Languages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
