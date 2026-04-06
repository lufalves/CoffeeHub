using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleAndNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Role",
                table: "Users");

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
        }
    }
}
