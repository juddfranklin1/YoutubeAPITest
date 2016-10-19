using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestBootstrapWebApp.Data.Migrations
{
    public partial class CreateYoutubeSchema : Migration
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
    }
}
