using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sem2week1.Migrations
{
    /// <inheritdoc />
    public partial class IdentityRolesSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0685625e-c498-4097-b6b3-9162715c0712", "8d9a770e-fc50-4f7b-95cb-36cef6fb9ea7", "Admin", "ADMIN" },
                    { "39ea0a6d-b66e-4e80-8da7-99204912d51a", "e38c7fac-2ab1-4235-9b20-0e18276ec2a7", "Student", "STUDENT" },
                    { "4fa0ab1b-08c1-4fb6-ae52-0fd090782cf8", "768462a2-b274-4245-9037-13bccb82e5f6", "Instructor", "INSTRUCTOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "0685625e-c498-4097-b6b3-9162715c0712");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "39ea0a6d-b66e-4e80-8da7-99204912d51a");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4fa0ab1b-08c1-4fb6-ae52-0fd090782cf8");
        }
    }
}
