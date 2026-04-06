using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelAfterRefactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coffees_BeanVarieties_BeanVarietyId1",
                table: "Coffees");

            migrationBuilder.DropForeignKey(
                name: "FK_Coffees_Farms_FarmId1",
                table: "Coffees");

            migrationBuilder.DropForeignKey(
                name: "FK_Coffees_Origins_OriginId1",
                table: "Coffees");

            migrationBuilder.DropForeignKey(
                name: "FK_Coffees_RoastLevels_RoastLevelId1",
                table: "Coffees");

            migrationBuilder.DropForeignKey(
                name: "FK_Coffees_Roasteries_RoasteryId1",
                table: "Coffees");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_BrewingMethods_BrewingMethodId1",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Coffees_CoffeeId1",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Coffees_CoffeeId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_CoffeeId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_BrewingMethodId1",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_CoffeeId1",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Coffees_BeanVarietyId1",
                table: "Coffees");

            migrationBuilder.DropIndex(
                name: "IX_Coffees_FarmId1",
                table: "Coffees");

            migrationBuilder.DropIndex(
                name: "IX_Coffees_OriginId1",
                table: "Coffees");

            migrationBuilder.DropIndex(
                name: "IX_Coffees_RoasteryId1",
                table: "Coffees");

            migrationBuilder.DropIndex(
                name: "IX_Coffees_RoastLevelId1",
                table: "Coffees");

            migrationBuilder.DropColumn(
                name: "CoffeeId1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "BrewingMethodId1",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "CoffeeId1",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "BeanVarietyId1",
                table: "Coffees");

            migrationBuilder.DropColumn(
                name: "FarmId1",
                table: "Coffees");

            migrationBuilder.DropColumn(
                name: "OriginId1",
                table: "Coffees");

            migrationBuilder.DropColumn(
                name: "RoastLevelId1",
                table: "Coffees");

            migrationBuilder.DropColumn(
                name: "RoasteryId1",
                table: "Coffees");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "CoffeeId1",
                table: "Reviews",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BrewingMethodId1",
                table: "Recipes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CoffeeId1",
                table: "Recipes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BeanVarietyId1",
                table: "Coffees",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FarmId1",
                table: "Coffees",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OriginId1",
                table: "Coffees",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoastLevelId1",
                table: "Coffees",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoasteryId1",
                table: "Coffees",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CoffeeId1",
                table: "Reviews",
                column: "CoffeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_BrewingMethodId1",
                table: "Recipes",
                column: "BrewingMethodId1");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CoffeeId1",
                table: "Recipes",
                column: "CoffeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Coffees_BeanVarietyId1",
                table: "Coffees",
                column: "BeanVarietyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Coffees_FarmId1",
                table: "Coffees",
                column: "FarmId1");

            migrationBuilder.CreateIndex(
                name: "IX_Coffees_OriginId1",
                table: "Coffees",
                column: "OriginId1");

            migrationBuilder.CreateIndex(
                name: "IX_Coffees_RoasteryId1",
                table: "Coffees",
                column: "RoasteryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Coffees_RoastLevelId1",
                table: "Coffees",
                column: "RoastLevelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Coffees_BeanVarieties_BeanVarietyId1",
                table: "Coffees",
                column: "BeanVarietyId1",
                principalTable: "BeanVarieties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Coffees_Farms_FarmId1",
                table: "Coffees",
                column: "FarmId1",
                principalTable: "Farms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Coffees_Origins_OriginId1",
                table: "Coffees",
                column: "OriginId1",
                principalTable: "Origins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Coffees_RoastLevels_RoastLevelId1",
                table: "Coffees",
                column: "RoastLevelId1",
                principalTable: "RoastLevels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Coffees_Roasteries_RoasteryId1",
                table: "Coffees",
                column: "RoasteryId1",
                principalTable: "Roasteries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_BrewingMethods_BrewingMethodId1",
                table: "Recipes",
                column: "BrewingMethodId1",
                principalTable: "BrewingMethods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Coffees_CoffeeId1",
                table: "Recipes",
                column: "CoffeeId1",
                principalTable: "Coffees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Coffees_CoffeeId1",
                table: "Reviews",
                column: "CoffeeId1",
                principalTable: "Coffees",
                principalColumn: "Id");
        }
    }
}
