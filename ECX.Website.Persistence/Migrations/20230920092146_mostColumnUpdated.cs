using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECX.Website.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mostColumnUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "ImgName",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FeedBacks");

            migrationBuilder.RenameColumn(
                name: "ImgName",
                table: "Videos",
                newName: "VideoName");

            migrationBuilder.RenameColumn(
                name: "ImgName",
                table: "TrainingDocs",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Subscriptions",
                newName: "SubscriberName");

            migrationBuilder.RenameColumn(
                name: "LangId",
                table: "Subscriptions",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "ImgName",
                table: "Researchs",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "ImgName",
                table: "Publications",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Messages",
                newName: "Subject");

            migrationBuilder.RenameColumn(
                name: "ImgName",
                table: "Messages",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "FeedBacks",
                newName: "Subject");

            migrationBuilder.RenameColumn(
                name: "ImgName",
                table: "FeedBacks",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Faqs",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "ImgName",
                table: "Faqs",
                newName: "Question");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Faqs",
                newName: "Answer");

            migrationBuilder.RenameColumn(
                name: "ImgName",
                table: "ExternalLinks",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "ImgName",
                table: "ContractFiles",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "ImgName",
                table: "Brochures",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Blogs",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Blogs",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Applicants",
                newName: "LName");

            migrationBuilder.RenameColumn(
                name: "LangId",
                table: "Applicants",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "ImgName",
                table: "Applicants",
                newName: "FName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Applicants",
                newName: "EduStatus");

            migrationBuilder.RenameColumn(
                name: "ImgName",
                table: "Announcements",
                newName: "FileName");

            migrationBuilder.AddColumn<string>(
                name: "Capacity",
                table: "WareHouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "WareHouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "WareHouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "SocialMedias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "Languages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommodityId",
                table: "ContractFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FName",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LName",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "WareHouses");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "WareHouses");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "WareHouses");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "SocialMedias");

            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "CommodityId",
                table: "ContractFiles");

            migrationBuilder.DropColumn(
                name: "FName",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "LName",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "VideoName",
                table: "Videos",
                newName: "ImgName");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "TrainingDocs",
                newName: "ImgName");

            migrationBuilder.RenameColumn(
                name: "SubscriberName",
                table: "Subscriptions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Subscriptions",
                newName: "LangId");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Researchs",
                newName: "ImgName");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Publications",
                newName: "ImgName");

            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "Messages",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Messages",
                newName: "ImgName");

            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "FeedBacks",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "FeedBacks",
                newName: "ImgName");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Faqs",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Question",
                table: "Faqs",
                newName: "ImgName");

            migrationBuilder.RenameColumn(
                name: "Answer",
                table: "Faqs",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "ExternalLinks",
                newName: "ImgName");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "ContractFiles",
                newName: "ImgName");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Brochures",
                newName: "ImgName");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Blogs",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Blogs",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "LName",
                table: "Applicants",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Applicants",
                newName: "LangId");

            migrationBuilder.RenameColumn(
                name: "FName",
                table: "Applicants",
                newName: "ImgName");

            migrationBuilder.RenameColumn(
                name: "EduStatus",
                table: "Applicants",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Announcements",
                newName: "ImgName");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImgName",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FeedBacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
