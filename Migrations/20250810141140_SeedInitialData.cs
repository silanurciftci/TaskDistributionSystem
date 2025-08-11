using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskDistributionSystem.Migrations
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Islemler",
                columns: new[] { "Id", "Ad", "Zorluk" },
                values: new object[] { 1, "Talep İnceleme", 1 });

            migrationBuilder.InsertData(
                table: "Islemler",
                columns: new[] { "Id", "Ad", "Zorluk" },
                values: new object[] { 2, "Hata Düzeltme", 2 });

            migrationBuilder.InsertData(
                table: "Islemler",
                columns: new[] { "Id", "Ad", "Zorluk" },
                values: new object[] { 3, "Küçük Geliştirme", 3 });

            migrationBuilder.InsertData(
                table: "Islemler",
                columns: new[] { "Id", "Ad", "Zorluk" },
                values: new object[] { 4, "Orta Geliştirme", 4 });

            migrationBuilder.InsertData(
                table: "Islemler",
                columns: new[] { "Id", "Ad", "Zorluk" },
                values: new object[] { 5, "Büyük Geliştirme", 5 });

            migrationBuilder.InsertData(
                table: "Islemler",
                columns: new[] { "Id", "Ad", "Zorluk" },
                values: new object[] { 6, "Kod İnceleme", 6 });

            migrationBuilder.InsertData(
                table: "Islemler",
                columns: new[] { "Id", "Ad", "Zorluk" },
                values: new object[] { 7, "Refactor", 7 });

            migrationBuilder.InsertData(
                table: "Islemler",
                columns: new[] { "Id", "Ad", "Zorluk" },
                values: new object[] { 8, "Performans İyileş.", 8 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Islemler",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Islemler",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Islemler",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Islemler",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Islemler",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Islemler",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Islemler",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Islemler",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
