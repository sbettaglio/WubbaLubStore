using Microsoft.EntityFrameworkCore.Migrations;

namespace WubbaLubStore.Migrations
{
    public partial class AddedFixedNavigationProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemOrders_ItemOrders_ItemOrderId",
                table: "ItemOrders");

            migrationBuilder.DropIndex(
                name: "IX_ItemOrders_ItemOrderId",
                table: "ItemOrders");

            migrationBuilder.DropColumn(
                name: "ItemOrderId",
                table: "ItemOrders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemOrderId",
                table: "ItemOrders",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemOrders_ItemOrderId",
                table: "ItemOrders",
                column: "ItemOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemOrders_ItemOrders_ItemOrderId",
                table: "ItemOrders",
                column: "ItemOrderId",
                principalTable: "ItemOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
