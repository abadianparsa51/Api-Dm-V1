using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserApi.Migrations
{
    /// <inheritdoc />
    public partial class ContactDestinationCardNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DestinationCardNumber",
                table: "Contacts",
                type: "nvarchar(16)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ContactId",
                table: "CardDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardDetails_ContactId",
                table: "CardDetails",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardDetails_Contacts_ContactId",
                table: "CardDetails",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardDetails_Contacts_ContactId",
                table: "CardDetails");

            migrationBuilder.DropIndex(
                name: "IX_CardDetails_ContactId",
                table: "CardDetails");

            migrationBuilder.DropColumn(
                name: "DestinationCardNumber",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "CardDetails");
        }
    }
}
