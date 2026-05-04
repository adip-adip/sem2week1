using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sem2week1.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Students",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2b481926-6bc8-4107-92c0-f56b947d4b8c", "3fc0ce6c-3d9c-41ef-9a43-6410e93c750b", "Student", "STUDENT" },
                    { "3a65aca8-bcf1-483f-9b4b-0e22e3c60d2e", "07ced6f4-13f1-44c3-964e-9a7ef942231a", "Instructor", "INSTRUCTOR" },
                    { "ca152055-daf8-47a3-b026-680bf188f7a8", "a95f9f19-e223-4273-8437-5bd8a84fdaff", "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Users_UserId",
                table: "Students",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Users_UserId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_UserId",
                table: "Students");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2b481926-6bc8-4107-92c0-f56b947d4b8c");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "3a65aca8-bcf1-483f-9b4b-0e22e3c60d2e");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "ca152055-daf8-47a3-b026-680bf188f7a8");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Students");

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
    }
}
