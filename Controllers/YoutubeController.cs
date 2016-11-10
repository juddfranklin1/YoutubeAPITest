using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using YoutubeAPIImplementation;
using TestBootstrapWebApp.Models;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace TestBootstrapWebApp.Controllers
{
    public class YoutubeController : Controller
    {
        NameBehavior NameStuff = new NameBehavior();

        public IActionResult YoutubeAPISearch()
        {
            return View();
        }
        private Dictionary<string,string> BuildArgumentsDictionary(ICollection<string> arg)
        {
            Dictionary<string,string> ArgumentsDictionary = new Dictionary<string,string>();

            foreach (String token in arg)
            {
                if (String.IsNullOrEmpty(Request.Form[token])){
                    ArgumentsDictionary.Add(token,"");                    
                }
                else {
                    ArgumentsDictionary.Add(token,Request.Form[token]);
                }
            }
            return ArgumentsDictionary;
        }

        private string BuildSearchResult(Dictionary<string,Dictionary<string,dynamic>> SearchResults)
        {
            StringBuilder SearchResultString = new StringBuilder();
            int Incrementer = 0;

            foreach(KeyValuePair<string, Dictionary<string,dynamic>> entry in SearchResults)
            {
                string EmbedCodeString = String.Format("<iframe src='https://www.youtube.com/embed/{0}' frameborder='0' allowfullscreen></iframe>", entry.Key);
                
                if(Incrementer %2 == 0){
                    SearchResultString.Append("<div class='row'>");
                }

                if (entry.Key == "NO_ID"){
                    SearchResultString.Append("<div class='col-md-12'><div class='alert-danger'>There was an error in your search.</div></div></div>");
                    return SearchResultString.ToString();
                }
                string FormattedPublishedDate = entry.Value["snippet"].PublishedAt.ToString();

                TimeSpan videoDuration = XmlConvert.ToTimeSpan(entry.Value["contentDetails"].Duration);

                SearchResultString.AppendFormat("<div class='col-sm-6'><h3 class='video-title'>{0}</h3><h4 class='video-upload-time'>{1}</h4><h6 class='video-duration'>{2}</h6><p class='video-description'>{3}</p><div class='video-embed'>{4}</div></div>",entry.Value["snippet"].Title,FormattedPublishedDate,videoDuration.ToString(),entry.Value["snippet"].Description,EmbedCodeString);
                if(Incrementer %2 == 1){
                    SearchResultString.Append("</div>");
                }
                Incrementer++;
            }
            return SearchResultString.ToString();
        }

        private string ConductSearch(ICollection<string> arg)
        {
            Dictionary<string,string> ArgumentsDictionary = BuildArgumentsDictionary(arg);

            YoutubeAPISearch ThisSearch = new YoutubeAPISearch();
            Dictionary<string,Dictionary<string,dynamic>> SearchResults = ThisSearch.YoutubeSearchAsync(ArgumentsDictionary).Result;
            
            string SearchResultString = BuildSearchResult(SearchResults);

            return SearchResultString;
        }

        [HttpPost]
        public IActionResult YoutubeSearch()
        {
            string SearchResultString = ConductSearch(Request.Form.Keys);

            ViewData["videoSearch"] = @SearchResultString;

            return View();
        }
        
        [HttpPost]
        public IActionResult YoutubeSearchAjax()
        {
            string SearchResultString = ConductSearch(Request.Form.Keys);

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