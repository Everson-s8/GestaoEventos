using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoEventos.Migrations
{
    /// <inheritdoc />
    public partial class nome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "EventProducts");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "EventStaffs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "EventProducts",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "EventStaffs");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "EventProducts");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "EventProducts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
