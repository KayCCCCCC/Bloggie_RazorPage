using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloggie.Migrations.AuthDb
{
    public partial class AddingNormalizedUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "206b8d0c-ad8a-46b7-ba0e-b4cd4b2b549f",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0150dda3-9381-4723-b711-c5204d9d5007", "SUPERADMIN@BLOGGIE.COM", "SUPERADMIN@BLOGGIE.COM", "AQAAAAIAAYagAAAAEK/cR9VJ/ix0SrdlkNHE6OpfTeJYrn8BvJBVtkXSc3wakCh16OcpOjgE3MMydr1Esw==", "df828c5d-bb99-4a33-a543-64ab820b1d40" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "206b8d0c-ad8a-46b7-ba0e-b4cd4b2b549f",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1b195260-16a1-4625-80e7-9b0dfa7e17ee", null, null, "AQAAAAIAAYagAAAAEAJ6/JvJ9d+XwC9DvdOYwkVQaeUT39A4MJrncvGZeeGpnIcXXnSGy9P4XZBAk8g9jQ==", "b77a4152-f05a-4bc5-92c8-10f896f343db" });
        }
    }
}
