using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RubiconTask.Migrations
{
    public partial class DataSeedInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BlogPosts",
                columns: new[] { "Id", "Body", "CreatedAt", "Description", "Slug", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "This is migrated data, feel free to delete it", new DateTime(2020, 8, 13, 17, 52, 16, 901, DateTimeKind.Local).AddTicks(7651), "We're migrating data so everyone who starts the app has it!", "migrating-data", "Migrating data", new DateTime(2020, 8, 13, 17, 52, 16, 906, DateTimeKind.Local).AddTicks(5531) },
                    { 2, "This is another migrated blogpost so we have more now", new DateTime(2020, 8, 13, 17, 52, 16, 906, DateTimeKind.Local).AddTicks(6842), "We're migrating some more data so it looks a bit richer", "another-migrated-data", "Another Migrated Data", new DateTime(2020, 8, 13, 17, 52, 16, 906, DateTimeKind.Local).AddTicks(6889) }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Ios" },
                    { 2, "AngularJS" },
                    { 3, "Migration" }
                });

            migrationBuilder.InsertData(
                table: "BlogPostTags",
                columns: new[] { "BlogPostId", "TagId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 1, 2 },
                    { 2, 3 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BlogPostTags",
                keyColumns: new[] { "BlogPostId", "TagId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "BlogPostTags",
                keyColumns: new[] { "BlogPostId", "TagId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "BlogPostTags",
                keyColumns: new[] { "BlogPostId", "TagId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "BlogPostTags",
                keyColumns: new[] { "BlogPostId", "TagId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
