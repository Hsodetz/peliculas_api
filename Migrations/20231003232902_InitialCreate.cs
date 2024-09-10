using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeliculasAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9aae0b6d-d50c-4d0a-9b90-2a6873e3845d",
                column: "ConcurrencyStamp",
                value: "b28c7526-5e7e-47fd-8548-0dd3738c5760");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5673b8cf-12de-44f6-92ad-fae4a77932ad",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "be02eaa3-d3cd-43f4-b510-a7617626bbf7", "AQAAAAEAACcQAAAAEA0pnZqeZK6yCMuqoxPdOxmGlbmczBJHrHwWwMpAptQgy30gtH8+OYGww4bZU4miDQ==", "d5b8e938-0fb8-4012-a285-9f9edcdca3ae" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9aae0b6d-d50c-4d0a-9b90-2a6873e3845d",
                column: "ConcurrencyStamp",
                value: "da9b005d-fe54-40fe-bc75-7b7bebd39526");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5673b8cf-12de-44f6-92ad-fae4a77932ad",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b22a7413-c832-4b53-ad5a-af69b38774e8", "AQAAAAEAACcQAAAAENwOcZXHcy72ZR2BXJBcjmd1k3V6lf+n5wHtEPKwEBfS3qx9hgJwqYAm2JXo9oeeSg==", "86bfeeb7-4a36-4cd6-9135-a8222045d1ae" });
        }
    }
}
