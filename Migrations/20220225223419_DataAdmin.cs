using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeliculasAPI.Migrations
{
    public partial class DataAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9aae0b6d-d50c-4d0a-9b90-2a6873e3845d", "af417122-d2c5-45be-b5bc-38c018ca1a61", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "5673b8cf-12de-44f6-92ad-fae4a77932ad", 0, "16541b76-2a7f-4767-b56b-2415a7b482a6", "felipe@hotmail.com", false, false, null, "felipe@hotmail.com", "felipe@hotmail.com", "AQAAAAEAACcQAAAAEF8B9m4qVC9H9IzVEN1ffxR5wqVYA6XT0HbXLxuOioUhjptDJHooADNzQogmy83bfQ==", null, false, "1f25044f-7f74-4d54-a5a6-788d68165b60", false, "felipe@hotmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "5673b8cf-12de-44f6-92ad-fae4a77932ad" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9aae0b6d-d50c-4d0a-9b90-2a6873e3845d");

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5673b8cf-12de-44f6-92ad-fae4a77932ad");
        }
    }
}
