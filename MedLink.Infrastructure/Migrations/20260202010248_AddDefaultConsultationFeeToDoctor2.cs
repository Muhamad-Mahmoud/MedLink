using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultConsultationFeeToDoctor2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ConsultationFee",
                table: "Doctors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 200m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ConsultationFee",
                table: "Doctors",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 200m);
        }

    }
}
