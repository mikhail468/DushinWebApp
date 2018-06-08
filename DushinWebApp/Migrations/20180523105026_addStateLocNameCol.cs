using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DushinWebApp.Migrations
{
    public partial class addStateLocNameCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocName",
                table: "PackageTbl",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocState",
                table: "PackageTbl",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocName",
                table: "PackageTbl");

            migrationBuilder.DropColumn(
                name: "LocState",
                table: "PackageTbl");
        }
    }
}
