using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BilHealth.Migrations
{
    public partial class InterfaceCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_AspNetUsers_NurseUserId",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_NurseUserId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "NurseUserId",
                table: "Cases");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications");

            migrationBuilder.AddColumn<Guid>(
                name: "NurseUserId",
                table: "Cases",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cases_NurseUserId",
                table: "Cases",
                column: "NurseUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_AspNetUsers_NurseUserId",
                table: "Cases",
                column: "NurseUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
