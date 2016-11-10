using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using System.Reflection;
using System.Threading;
using System.Text;

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
        private const string ApiKeyString = "AIzaSyBnAloJ18VwhKYgtz8qK9FYxRnpZPKkA04";

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
        public async Task<Dictionary<string,Dictionary<string,dynamic>>> YoutubeSearchAsync(Dictionary<string,string> arg)
        {
            Dictionary<string,Dictionary<string,dynamic>> ConstructedResults = new Dictionary<string,Dictionary<string,dynamic>>();
            try
            {
                Task<Dictionary<string,SearchResultSnippet>> SearchResults = new YoutubeAPISearch().RunSearch(arg);
                SearchResults.Wait();
                Dictionary<string,SearchResultSnippet> Results = await SearchResults;
                if (Results.Count() > 0)
                {
                    List<string> videoIds = new List<string>();
                    foreach (string videoId in Results.Keys)
                    {
                        videoIds.Add(videoId);
                        ConstructedResults.Add(videoId,new Dictionary<string,dynamic>());
                        ConstructedResults[videoId]["snippet"] = Results[videoId];
                    }
                    
                    Task<List<Video>> VideoDetails = new YoutubeAPISearch().RunVideoLookup(videoIds);

                    VideoDetails.Wait();
                    
                    List<Video> VideoDetailResults = await VideoDetails;

                    foreach (Video videoResult in VideoDetailResults)
                    {
                        TimeSpan videoDuration = XmlConvert.ToTimeSpan(videoResult.ContentDetails.Duration);

                        ConstructedResults[videoResult.Id]["contentDetails"] = videoResult.ContentDetails;
                        ConstructedResults[videoResult.Id]["videoDurationString"] = videoDuration.ToString();
                    }
                }

                return ConstructedResults;
            }
            catch (AggregateException ex)
            {
                Dictionary<string,dynamic> Errors = new Dictionary<string,dynamic>();
                Dictionary<string,Dictionary<string,dynamic>> Results = new Dictionary<string,Dictionary<string,dynamic>>();
                
                int Counter = 0;
                
                SearchResultSnippet errorResult = new SearchResultSnippet();

                StringBuilder errorMessages = new StringBuilder("Errors");

                foreach (var e in ex.InnerExceptions)
                {   
                    errorMessages.AppendFormat("\nError #{0}: {1}", Counter.ToString(), e.ToString());
                    Counter++;
                }
                errorResult.Title = "***YOUTUBE SEARCH EXCEPTIONS THROWN***";   
                errorResult.Description = errorMessages.ToString();
                Errors.Add("Exceptions: ", errorResult);
                Results.Add("NO_ID", Errors);
                return Results;
            }
        }

        private async Task<List<Video>> RunVideoLookup(List<string> arg)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = ApiKeyString,
                ApplicationName = this.GetType().ToString()
            });

            var videoListRequest = youtubeService.Videos.List("snippet,contentDetails,statistics,status");
            StringBuilder idList = new StringBuilder();
            int Iterator = 1;
            foreach (string id in arg) {
                idList.Append(id);
                if (Iterator < arg.Count())
                {
                    idList.Append(',');
                }
                Iterator++;
            }
            videoListRequest.Id = idList.ToString();

            var videoListResponse = await videoListRequest.ExecuteAsync();
            List<Video> videos = videoListResponse.Items.ToList();

            return videos;
        }

        private async Task<Dictionary<string,SearchResultSnippet>> RunSearch(Dictionary<string,string> arg)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = ApiKeyString,
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            if (String.IsNullOrEmpty(arg["SearchTerm"])){
                searchListRequest.Q = "dotnet tutorials"; // Default
            } else {
                searchListRequest.Q = arg["SearchTerm"];
            }
            
            SearchSettings Settings = new SearchSettings();

            searchListRequest.Type = "video";
            searchListRequest.Order = Settings.SetOrderSetting(arg);
            searchListRequest.VideoDefinition = Settings.SetDefinitionSetting(arg);
            searchListRequest.VideoDimension = Settings.SetDimensionSetting(arg);
            searchListRequest.VideoDuration = Settings.SetDurationSetting(arg);
            
            searchListRequest.MaxResults = 10;
            searchListRequest.VideoEmbeddable = SearchResource.ListRequest.VideoEmbeddableEnum.True__;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();
            Dictionary<string,SearchResultSnippet> videos = new Dictionary<string,SearchResultSnippet>();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var individualSearchResult in searchListResponse.Items)
            {
                videos.Add(individualSearchResult.Id.VideoId,individualSearchResult.Snippet);
            }
                        
            return videos;
        }

    }
}