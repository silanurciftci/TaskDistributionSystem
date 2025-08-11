using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskDistributionSystem.Migrations
{
    public partial class SeedPersonel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Personeller",
                columns: new[] { "Id", "Ad", "Rol", "Soyad" },
                values: new object[] { 1, "Ali", 0, "Yılmaz" });

            migrationBuilder.InsertData(
                table: "Personeller",
                columns: new[] { "Id", "Ad", "Rol", "Soyad" },
                values: new object[] { 2, "Ayşe", 0, "Demir" });

            migrationBuilder.InsertData(
                table: "Personeller",
                columns: new[] { "Id", "Ad", "Rol", "Soyad" },
                values: new object[] { 3, "Mehmet", 0, "Kaya" });

            migrationBuilder.InsertData(
                table: "Personeller",
                columns: new[] { "Id", "Ad", "Rol", "Soyad" },
                values: new object[] { 4, "Elif", 0, "Çetin" });

            migrationBuilder.InsertData(
                table: "Personeller",
                columns: new[] { "Id", "Ad", "Rol", "Soyad" },
                values: new object[] { 5, "Can", 0, "Aksoy" });

            migrationBuilder.InsertData(
                table: "Personeller",
                columns: new[] { "Id", "Ad", "Rol", "Soyad" },
                values: new object[] { 6, "Zeynep", 0, "Şahin" });

            migrationBuilder.InsertData(
                table: "Personeller",
                columns: new[] { "Id", "Ad", "Rol", "Soyad" },
                values: new object[] { 7, "Burak", 0, "Koç" });

            migrationBuilder.InsertData(
                table: "Personeller",
                columns: new[] { "Id", "Ad", "Rol", "Soyad" },
                values: new object[] { 8, "Ece", 0, "Aydın" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Personeller",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Personeller",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Personeller",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Personeller",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Personeller",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Personeller",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Personeller",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Personeller",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
