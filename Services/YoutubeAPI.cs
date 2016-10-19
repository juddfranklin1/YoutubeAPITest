using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using System.Reflection;
using System.Threading;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YoutubeAPIImplementation
{

    public class YoutubeAPISearch
    {

        /// <summary>
        /// YouTube Data API v3 sample: search by keyword.
        /// Relies on the Google APIs Client Library for .NET, v1.7.0 or higher.
        /// See https://code.google.com/p/google-api-dotnet-client/wiki/GettingStarted
        ///
        /// Set ApiKey to the API key value from the APIs & auth > Registered apps tab of
        ///   https://cloud.google.com/console
        /// Please ensure that you have enabled the YouTube Data API for your project.
        /// </summary>
        [STAThread]
        public async Task<Dictionary<string,string>> YoutubeSearchAsync(Dictionary<string,dynamic> arg)
        {
            try
            {
                Task<Dictionary<string,string>> SearchResults = new YoutubeAPISearch().Run(arg);
                SearchResults.Wait();
                Dictionary<string,string> Results = await SearchResults;

                return Results;
            }
            catch (AggregateException ex)
            {
                Dictionary<string,string> Results = new Dictionary<string,string>();
                int Counter = 0;
                foreach (var e in ex.InnerExceptions)
                {
                    Results.Add("Error #" + Counter, e.Message);
                    Counter++;
                }
                return Results;
            }
        }

        private async Task<Dictionary<string,string>> Run(Dictionary<string,dynamic> arg)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBnAloJ18VwhKYgtz8qK9FYxRnpZPKkA04",
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            if (String.IsNullOrEmpty(arg["SearchTerm"])){
                searchListRequest.Q = "dotnet tutorials"; // Default
            } else {
                searchListRequest.Q = arg["SearchTerm"];
            }

            if (String.IsNullOrEmpty(arg["Order"])){
                searchListRequest.Order = searchListRequest.Order.Value; // Default
            } else {
                string OrderString = arg["Order"];
                switch(OrderString){
                    case ("Relevance"):
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;
                        break;
                    case ("Date"):
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
                        break;
                    case ("Rating"):
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Rating;
                        break;
                    case ("Title"):
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Title;
                        break;
                    case ("ViewCount"):
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.ViewCount;
                        break;
                    default:
                        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;
                        break;                                                                                              
                }
            }
            
            searchListRequest.Type = "video";
            searchListRequest.MaxResults = 10;
            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            Dictionary<string,string> videos = new Dictionary<string,string>();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            int Incrementer = 1;
            foreach (var searchResult in searchListResponse.Items)
            {
                if(!videos.ContainsKey(searchResult.Snippet.Title)){
                    videos.Add(String.Format("{0}", searchResult.Snippet.Title),String.Format("<iframe width='420' height='315' src='https://www.youtube.com/embed/{1}' frameborder='0' allowfullscreen></iframe>", searchResult.Snippet.Title, searchResult.Id.VideoId));
                } else {
                    videos.Add(String.Format("{0} #{1}", searchResult.Snippet.Title,Incrementer),String.Format("<iframe width='420' height='315' src='https://www.youtube.com/embed/{1}' frameborder='0' allowfullscreen></iframe>", searchResult.Snippet.Title, searchResult.Id.VideoId));
                    Incrementer++;
                }
            }

            return videos;
        }
        
    }
}