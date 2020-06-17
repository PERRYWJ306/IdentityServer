using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroActive.Security.AuthorizationServer.Data.Migrations
{
    public partial class AddTenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationTenant",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 450, nullable: false),
                    BuildingNameNo = table.Column<string>(unicode: false, nullable: true),
                    County = table.Column<string>(unicode: false, nullable: true),
                    Email = table.Column<string>(unicode: false, nullable: true),
                    Fax = table.Column<string>(unicode: false, nullable: true),
                    Logo = table.Column<string>(unicode: false, nullable: true),
                    Name = table.Column<string>(unicode: false, nullable: true),
                    PostalCode = table.Column<string>(unicode: false, nullable: true),
                    RegistrationNumber = table.Column<string>(unicode: false, nullable: true),
                    StreetName = table.Column<string>(unicode: false, nullable: true),
                    Telephone = table.Column<string>(unicode: false, nullable: true),
                    TownCity = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTenant", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ApplicationTenant_TenantId",
                table: "AspNetUsers",
                column: "TenantId",
                principalTable: "ApplicationTenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ApplicationTenant_TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ApplicationTenant");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AspNetUsers");
        }
    }
}
