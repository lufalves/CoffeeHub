using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationshipConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RoastLevels_Name",
                table: "RoastLevels");

            migrationBuilder.DropIndex(
                name: "IX_Roasteries_Name",
                table: "Roasteries");

            migrationBuilder.DropIndex(
                name: "IX_Origins_Country_Region_Locality",
                table: "Origins");

            migrationBuilder.DropIndex(
                name: "IX_Farms_Name_OriginId",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_BrewingMethods_Name",
                table: "BrewingMethods");

            migrationBuilder.DropIndex(
                name: "IX_BeanVarieties_Name",
                table: "BeanVarieties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RoastLevels_Name",
                table: "RoastLevels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roasteries_Name",
                table: "Roasteries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Origins_Country_Region_Locality",
                table: "Origins",
                columns: new[] { "Country", "Region", "Locality" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farms_Name_OriginId",
                table: "Farms",
                columns: new[] { "Name", "OriginId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrewingMethods_Name",
                table: "BrewingMethods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BeanVarieties_Name",
                table: "BeanVarieties",
                column: "Name",
                unique: true);
        }
    }
}
