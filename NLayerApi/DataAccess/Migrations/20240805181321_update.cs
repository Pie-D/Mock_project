using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Addresses_AddressId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_CompanyContacts_CompanyContactId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "SupportingMaterials");

            migrationBuilder.AddColumn<int>(
                name: "CompanyContactId",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "SupportingMaterials",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AddColumn<string>(
                name: "GovOfficeRegionName",
                table: "Organisations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrustDistrictName",
                table: "Organisations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrustRegionName",
                table: "Organisations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyContactId",
                table: "Departments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Departments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "OrganisationReferenceDatas",
                columns: table => new
                {
                    OrganisationId = table.Column<int>(type: "int", nullable: false),
                    RefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationReferenceDatas", x => new { x.OrganisationId, x.RefId });
                    table.ForeignKey(
                        name: "FK_OrganisationReferenceDatas_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganisationReferenceDatas_ReferenceDatas_RefId",
                        column: x => x.RefId,
                        principalTable: "ReferenceDatas",
                        principalColumn: "RefId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CompanyContactId",
                table: "Teams",
                column: "CompanyContactId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationReferenceDatas_RefId",
                table: "OrganisationReferenceDatas",
                column: "RefId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Addresses_AddressId",
                table: "Departments",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_CompanyContacts_CompanyContactId",
                table: "Departments",
                column: "CompanyContactId",
                principalTable: "CompanyContacts",
                principalColumn: "CompanyContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_CompanyContacts_CompanyContactId",
                table: "Teams",
                column: "CompanyContactId",
                principalTable: "CompanyContacts",
                principalColumn: "CompanyContactId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Addresses_AddressId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_CompanyContacts_CompanyContactId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_CompanyContacts_CompanyContactId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "OrganisationReferenceDatas");

            migrationBuilder.DropIndex(
                name: "IX_Teams_CompanyContactId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CompanyContactId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "GovOfficeRegionName",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "TrustDistrictName",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "TrustRegionName",
                table: "Organisations");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "SupportingMaterials",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "SupportingMaterials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyContactId",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Addresses_AddressId",
                table: "Departments",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_CompanyContacts_CompanyContactId",
                table: "Departments",
                column: "CompanyContactId",
                principalTable: "CompanyContacts",
                principalColumn: "CompanyContactId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
