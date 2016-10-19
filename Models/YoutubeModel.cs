using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TestBootstrapWebApp.Data;

namespace TestBootstrapWebApp.Models
{
    public class YoutubeSearch
    {
        public int ID { get; set; }
        public string SearchQuery { get; set; }
        public string SearchSort { get; set; }
        public string SearchResults { get; set; }
    }
}