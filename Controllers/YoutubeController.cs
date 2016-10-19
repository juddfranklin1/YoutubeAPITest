using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.YouTube.v3;

using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using YoutubeAPIImplementation;
using TestBootstrapWebApp.Models;

namespace TestBootstrapWebApp.Controllers
{
    public class YoutubeController : Controller
    {
        NameBehavior NameStuff = new NameBehavior();

        public IActionResult YoutubeAPISearch()
        {
            return View();
        }

        [HttpPost]
        public IActionResult YoutubeSearchQuery()
        {
            Dictionary<string,dynamic> Arguments = new Dictionary<string,dynamic>();

            string SearchTerm = Request.Form["SearchTerm"];
            
            if (String.IsNullOrEmpty(SearchTerm)){
                SearchTerm = "CAN Ege Bamyasi";
            }

            Arguments.Add("Q", SearchTerm);

            YoutubeAPISearch ThisSearch = new YoutubeAPIImplementation.YoutubeAPISearch();
            Dictionary<string,string> SearchResults = ThisSearch.YoutubeSearchAsync(Arguments).Result;
            string SearchResultString = String.Format("{0}\n", string.Join("\n", SearchResults));

            ViewData["videoSearch"] = @SearchResultString;

            return View();
        }
        
        [HttpPost]
        public IActionResult YoutubeSearchAjax()
        {
            Dictionary<string,dynamic> ArgumentsDictionary = new Dictionary<string,dynamic>();

            foreach (String token in Request.Form.Keys)
            {
                if (String.IsNullOrEmpty(Request.Form[token])){
                    ArgumentsDictionary.Add(token,"");                    
                }
                else {
                    ArgumentsDictionary.Add(token,Request.Form[token]);
                }
                Console.WriteLine("" + token + ": " + (string)(ArgumentsDictionary[token]));
            }

            YoutubeAPISearch ThisSearch = new YoutubeAPISearch();
            Dictionary<string,string> SearchResults = ThisSearch.YoutubeSearchAsync(ArgumentsDictionary).Result;

            string VideosString = "";
            
            foreach (var Video in SearchResults){
                VideosString += "<h3>" + Video.Key + "</h3>";
                VideosString += Video.Value;
            }

            string SearchResultString = String.Format("{0}\n", string.Join("\n", VideosString));

            return Json(SearchResultString);
        }

        // POST: /Manage/AddYoutubeSearch
        // [HttpPost]
        // public async Task<IActionResult> AddYoutubeSearch(YoutubeModel model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View(model);
        //     }
        // }
 
        public IActionResult Error()
        {
            return View();
        }

        private IHostingEnvironment hostingEnv;

        public YoutubeController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }
        
    }
}