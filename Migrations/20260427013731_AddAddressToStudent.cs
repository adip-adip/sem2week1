using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sem2week1.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Students",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2b8e206c-3db5-4636-9c9b-1063101a8bc4", "d29a0197-3226-4b73-b9d9-3867d8863b4a", "Student", "STUDENT" },
                    { "478a7f86-a632-4212-a303-814b1e622f9c", "cdf3fdbe-43f5-418a-8d0d-852bd66c8eef", "Admin", "ADMIN" },
                    { "7d313939-dee6-4a0e-91d3-f348a3bb09e9", "5de64264-5b7b-4c75-9914-0542814c55be", "Instructor", "INSTRUCTOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2b8e206c-3db5-4636-9c9b-1063101a8bc4");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "478a7f86-a632-4212-a303-814b1e622f9c");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "7d313939-dee6-4a0e-91d3-f348a3bb09e9");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Students");

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
    }
}
