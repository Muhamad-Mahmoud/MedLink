using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace MedLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConvertToLocationPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Specializations_SpecialtyId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Doctors");

            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Doctors",
                type: "geography",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Specializations_SpecialtyId",
                table: "Doctors",
                column: "SpecialtyId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Specializations_SpecialtyId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Doctors");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Doctors",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Doctors",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Specializations_SpecialtyId",
                table: "Doctors",
                column: "SpecialtyId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
