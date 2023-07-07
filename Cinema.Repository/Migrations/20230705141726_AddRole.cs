using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinema.Repository.Migrations
{
    public partial class AddRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInShoppingCarts_ShoppingCards_ShoppingCartId",
                table: "TicketsInShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_TicketsInShoppingCarts_ShoppingCartId",
                table: "TicketsInShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "TicketsInShoppingCarts");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsInShoppingCarts_ShoppingCards_CartId",
                table: "TicketsInShoppingCarts",
                column: "CartId",
                principalTable: "ShoppingCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInShoppingCarts_ShoppingCards_CartId",
                table: "TicketsInShoppingCarts");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "TicketsInShoppingCarts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketsInShoppingCarts_ShoppingCartId",
                table: "TicketsInShoppingCarts",
                column: "ShoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsInShoppingCarts_ShoppingCards_ShoppingCartId",
                table: "TicketsInShoppingCarts",
                column: "ShoppingCartId",
                principalTable: "ShoppingCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
