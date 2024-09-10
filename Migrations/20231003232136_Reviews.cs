using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeliculasAPI.Migrations
{
    public partial class Reviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Puntuacion = table.Column<int>(type: "int", nullable: false),
                    PeliculaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_Peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "b22a7413-c832-4b53-ad5a-af69b38774e8", "helmuth@gmail.com", "helmuth@gmail.com", "helmuth@gmail.com", "AQAAAAEAACcQAAAAENwOcZXHcy72ZR2BXJBcjmd1k3V6lf+n5wHtEPKwEBfS3qx9hgJwqYAm2JXo9oeeSg==", "86bfeeb7-4a36-4cd6-9135-a8222045d1ae", "helmuth@gmail.com" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PeliculaId",
                table: "Reviews",
                column: "PeliculaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UsuarioId",
                table: "Reviews",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9aae0b6d-d50c-4d0a-9b90-2a6873e3845d",
                column: "ConcurrencyStamp",
                value: "af417122-d2c5-45be-b5bc-38c018ca1a61");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5673b8cf-12de-44f6-92ad-fae4a77932ad",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "16541b76-2a7f-4767-b56b-2415a7b482a6", "felipe@hotmail.com", "felipe@hotmail.com", "felipe@hotmail.com", "AQAAAAEAACcQAAAAEF8B9m4qVC9H9IzVEN1ffxR5wqVYA6XT0HbXLxuOioUhjptDJHooADNzQogmy83bfQ==", "1f25044f-7f74-4d54-a5a6-788d68165b60", "felipe@hotmail.com" });
        }
    }
}
