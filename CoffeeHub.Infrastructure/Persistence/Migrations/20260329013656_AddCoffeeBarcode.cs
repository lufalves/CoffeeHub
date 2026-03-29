using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCoffeeBarcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Coffees",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Coffees_Barcode",
                table: "Coffees",
                column: "Barcode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Coffees_Barcode",
                table: "Coffees");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Coffees");
        }
    }
}
