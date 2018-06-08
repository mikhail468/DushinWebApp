using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DushinWebApp.Migrations
{
    public partial class addProviderProfileTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "ProfileTbl",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "ProfileTbl",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProviderProfileTbl",
                columns: table => new
                {
                    ProviderProfileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    WebSite = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderProfileTbl", x => x.ProviderProfileId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderProfileTbl");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "ProfileTbl");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "ProfileTbl");
        }
    }
}
