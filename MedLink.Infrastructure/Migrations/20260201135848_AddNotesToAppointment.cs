using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotesToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Appointments");
        }

    }
}
