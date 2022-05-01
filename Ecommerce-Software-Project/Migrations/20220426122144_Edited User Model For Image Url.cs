using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_Software_Project.Migrations
{
    public partial class EditedUserModelForImageUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProduct_Products_productId",
                table: "FavouriteProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProduct_Users_userId",
                table: "FavouriteProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavouriteProduct",
                table: "FavouriteProduct");

            migrationBuilder.RenameTable(
                name: "FavouriteProduct",
                newName: "FavouriteProducts");

            migrationBuilder.RenameColumn(
                name: "UserImage",
                table: "Users",
                newName: "UserImageUrl");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteProduct_userId",
                table: "FavouriteProducts",
                newName: "IX_FavouriteProducts_userId");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteProduct_productId",
                table: "FavouriteProducts",
                newName: "IX_FavouriteProducts_productId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavouriteProducts",
                table: "FavouriteProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProducts_Products_productId",
                table: "FavouriteProducts",
                column: "productId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProducts_Users_userId",
                table: "FavouriteProducts",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProducts_Products_productId",
                table: "FavouriteProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProducts_Users_userId",
                table: "FavouriteProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavouriteProducts",
                table: "FavouriteProducts");

            migrationBuilder.RenameTable(
                name: "FavouriteProducts",
                newName: "FavouriteProduct");

            migrationBuilder.RenameColumn(
                name: "UserImageUrl",
                table: "Users",
                newName: "UserImage");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteProducts_userId",
                table: "FavouriteProduct",
                newName: "IX_FavouriteProduct_userId");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteProducts_productId",
                table: "FavouriteProduct",
                newName: "IX_FavouriteProduct_productId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavouriteProduct",
                table: "FavouriteProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProduct_Products_productId",
                table: "FavouriteProduct",
                column: "productId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProduct_Users_userId",
                table: "FavouriteProduct",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
