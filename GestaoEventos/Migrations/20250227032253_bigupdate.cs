using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GestaoEventos.Migrations
{
    /// <inheritdoc />
    public partial class bigupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "EventProducts");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EventStaffs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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

            migrationBuilder.AddForeignKey(
                name: "FK_EventStaffs_Users_Id",
                table: "EventStaffs",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventStaffs_Users_Id",
                table: "EventStaffs");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "EventStaffs");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "EventProducts");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EventStaffs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "EventProducts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
