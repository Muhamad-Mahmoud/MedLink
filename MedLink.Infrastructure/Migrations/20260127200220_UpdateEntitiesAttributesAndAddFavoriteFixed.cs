using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntitiesAttributesAndAddFavoriteFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_UserId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorAvailabilities_AvailabilityId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Specializations_SpecializationId",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "SpecializationId",
                table: "Doctors",
                newName: "SpecialtyId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_SpecializationId",
                table: "Doctors",
                newName: "IX_Doctors_SpecialtyId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Appointments",
                newName: "BookedByUserId");

            migrationBuilder.RenameColumn(
                name: "AvailabilityId",
                table: "Appointments",
                newName: "ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_UserId",
                table: "Appointments",
                newName: "IX_Appointments_BookedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_AvailabilityId",
                table: "Appointments",
                newName: "IX_Appointments_ScheduleId");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Payments",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "Payments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Doctors",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "Appointments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientPhone",
                table: "Appointments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    MedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_DoctorId",
                table: "Favorites",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId_DoctorId",
                table: "Favorites",
                columns: new[] { "UserId", "DoctorId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_BookedByUserId",
                table: "Appointments",
                column: "BookedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorAvailabilities_ScheduleId",
                table: "Appointments",
                column: "ScheduleId",
                principalTable: "DoctorAvailabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Appointments_AspNetUsers_BookedByUserId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorAvailabilities_ScheduleId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Specializations_SpecialtyId",
                table: "Doctors");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PatientPhone",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "SpecialtyId",
                table: "Doctors",
                newName: "SpecializationId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_SpecialtyId",
                table: "Doctors",
                newName: "IX_Doctors_SpecializationId");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "Appointments",
                newName: "AvailabilityId");

            migrationBuilder.RenameColumn(
                name: "BookedByUserId",
                table: "Appointments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_ScheduleId",
                table: "Appointments",
                newName: "IX_Appointments_AvailabilityId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_BookedByUserId",
                table: "Appointments",
                newName: "IX_Appointments_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_UserId",
                table: "Appointments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorAvailabilities_AvailabilityId",
                table: "Appointments",
                column: "AvailabilityId",
                principalTable: "DoctorAvailabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Specializations_SpecializationId",
                table: "Doctors",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
