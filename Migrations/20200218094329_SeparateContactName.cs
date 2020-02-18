using Microsoft.EntityFrameworkCore.Migrations;

namespace authorizationRoles.Migrations
{
    public partial class SeparateContactName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contact",
                keyColumn: "ContactId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contact",
                keyColumn: "ContactId",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Contact");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Contact",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Contact",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Contact");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Contact",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "ContactId", "Address", "City", "Email", "Name", "OwnerID", "State", "Status", "Zip" },
                values: new object[] { 1, "1234 Main St", "Redmond", "debra@example.com", "Debra Garcia", null, "WA", 0, "10999" });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "ContactId", "Address", "City", "Email", "Name", "OwnerID", "State", "Status", "Zip" },
                values: new object[] { 2, "5678 1st Ave W", "Redmond", "thorsten@example.com", "Thorsten Weinrich", null, "WA", 0, "10999" });
        }
    }
}
