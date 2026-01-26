using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrenchRevolution.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalizedNameToCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Characters",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                "UPDATE \"Characters\" SET \"NormalizedName\" = LOWER(TRIM(\"Name\"))");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_NormalizedName",
                table: "Characters",
                column: "NormalizedName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Characters_NormalizedName",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Characters");
        }
    }
}
