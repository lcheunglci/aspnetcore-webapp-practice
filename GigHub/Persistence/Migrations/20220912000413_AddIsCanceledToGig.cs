using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigHub.Data.Migrations
{
    public partial class AddIsCanceledToGig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Gigs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Gigs");
        }
    }
}
