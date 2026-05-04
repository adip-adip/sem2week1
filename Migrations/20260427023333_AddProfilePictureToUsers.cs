using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sem2week1.Migrations
{
    /// <inheritdoc />
    public partial class AddProfilePictureToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "Users",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureContentType",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b1019eb2-2f92-4610-9faa-cf7859ddb1ff", "aa777695-b359-49e5-8308-b09ac07a43c8", "Instructor", "INSTRUCTOR" },
                    { "c6a4f91e-3141-4b9f-b53b-8abaf12baf24", "741d41e9-97b8-479b-adce-2034edf595eb", "Admin", "ADMIN" },
                    { "e65e5bdb-c052-49ca-9864-69387f3902a7", "fcebecc9-641d-4ac7-a705-21b74604c766", "Student", "STUDENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "b1019eb2-2f92-4610-9faa-cf7859ddb1ff");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "c6a4f91e-3141-4b9f-b53b-8abaf12baf24");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e65e5bdb-c052-49ca-9864-69387f3902a7");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfilePictureContentType",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2b481926-6bc8-4107-92c0-f56b947d4b8c", "3fc0ce6c-3d9c-41ef-9a43-6410e93c750b", "Student", "STUDENT" },
                    { "3a65aca8-bcf1-483f-9b4b-0e22e3c60d2e", "07ced6f4-13f1-44c3-964e-9a7ef942231a", "Instructor", "INSTRUCTOR" },
                    { "ca152055-daf8-47a3-b026-680bf188f7a8", "a95f9f19-e223-4273-8437-5bd8a84fdaff", "Admin", "ADMIN" }
                });
        }
    }
}
