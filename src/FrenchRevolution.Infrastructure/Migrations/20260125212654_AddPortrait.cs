using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrenchRevolution.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPortrait : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PortraitUrl",
                table: "Characters",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PortraitUrl",
                table: "Characters");
        }
    }
}
