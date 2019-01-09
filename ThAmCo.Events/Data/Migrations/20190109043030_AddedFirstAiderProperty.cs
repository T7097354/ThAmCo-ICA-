using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class AddedFirstAiderProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffBooking_Staff_StaffId",
                schema: "thamco.events",
                table: "StaffBooking");

            migrationBuilder.AddColumn<bool>(
                name: "FirstAider",
                schema: "thamco.events",
                table: "Staff",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffBooking_Staff_StaffId",
                schema: "thamco.events",
                table: "StaffBooking",
                column: "StaffId",
                principalSchema: "thamco.events",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffBooking_Staff_StaffId",
                schema: "thamco.events",
                table: "StaffBooking");

            migrationBuilder.DropColumn(
                name: "FirstAider",
                schema: "thamco.events",
                table: "Staff");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffBooking_Staff_StaffId",
                schema: "thamco.events",
                table: "StaffBooking",
                column: "StaffId",
                principalSchema: "thamco.events",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
