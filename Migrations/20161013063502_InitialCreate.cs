using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestBootstrapWebApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YoutubeSearches",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SearchQuery = table.Column<string>(nullable: true),
                    SearchSort = table.Column<string>(maxLength: 256, nullable: true),
                    SearchResults = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeSearches", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "SearchQueryIndex",
                table: "YoutubeSearches",
                column: "SearchQuery");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
