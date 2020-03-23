using System;
using InfectionWebApp.Data;
using InfectionWebApp.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InfectionWebApp.Migrations
{
    public partial class InitialCreaate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(nullable: false),
                    Confirmed = table.Column<int>(nullable: false),
                    Deaths = table.Column<int>(nullable: false),
                    Recovered = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projections", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projections");
        }
    }
}
