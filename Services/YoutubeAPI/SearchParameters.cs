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
    public class SearchSettings
    {
        public SearchSettings() { }
        public void SetSettings(Google.Apis.YouTube.v3.SearchResource.ListRequest request,Dictionary<string,string> arg)
        {
            Dictionary<string,dynamic> settingsDict = new Dictionary<string,dynamic>();

            SearchResource.ListRequest.VideoDefinitionEnum DefinitionSetting = SetDefinitionSetting(arg);
            SearchResource.ListRequest.VideoDimensionEnum DimensionSetting = SetDimensionSetting(arg);
            SearchResource.ListRequest.OrderEnum OrderSetting = SetOrderSetting(arg);
            SearchResource.ListRequest.VideoDurationEnum DurationSetting = SetDurationSetting(arg);
            
            request.VideoDefinition = DefinitionSetting;
            request.VideoDimension = DimensionSetting;
            request.Order = OrderSetting;
            request.VideoDuration = DurationSetting;
        }
        private SearchResource.ListRequest.VideoDefinitionEnum _definition;

        private SearchResource.ListRequest.VideoDefinitionEnum CurrentDefinition
        {
            get { return _definition; }
            set { _definition = value; }
        }

        private SearchResource.ListRequest.VideoDefinitionEnum SetDefinitionSetting(Dictionary<string,string> arg)
        {
            if (!arg.ContainsKey("VideoDefinition") || arg["VideoDefinition"] == "null")
            {
                CurrentDefinition = SearchResource.ListRequest.VideoDefinitionEnum.Any;
            } else {
                string ArguedDefinition = arg["VideoDefinition"];
                CurrentDefinition = (SearchResource.ListRequest.VideoDefinitionEnum)Enum.Parse(typeof(SearchResource.ListRequest.VideoDefinitionEnum),ArguedDefinition);
            }
            return CurrentDefinition;
        }


        private SearchResource.ListRequest.VideoDimensionEnum _dimension;

        private SearchResource.ListRequest.VideoDimensionEnum CurrentDimension
        {
            get { return _dimension; }
            set { _dimension = value; }
        }

        private SearchResource.ListRequest.VideoDimensionEnum SetDimensionSetting(Dictionary<string,string> arg)
        {
            if (!arg.ContainsKey("VideoDimension") || arg["VideoDimension"] == "null")
            {
                CurrentDimension = SearchResource.ListRequest.VideoDimensionEnum.Any;
            } else {
                string ArguedValue = arg["VideoDimension"];
                CurrentDimension = (SearchResource.ListRequest.VideoDimensionEnum)Enum.Parse(typeof(SearchResource.ListRequest.VideoDimensionEnum),ArguedValue);
            }
            return CurrentDimension;
        }


        private SearchResource.ListRequest.OrderEnum _order;

        private SearchResource.ListRequest.OrderEnum CurrentOrder
        {
            get { return _order; }
            set { _order = value; }
        }

        private SearchResource.ListRequest.OrderEnum SetOrderSetting(Dictionary<string,string> arg)
        {
            if (!arg.ContainsKey("Order") || arg["Order"] == "null")
            {
                CurrentOrder = SearchResource.ListRequest.OrderEnum.Relevance;
            } else {
                string ArguedValue = arg["Order"];
                CurrentOrder = (SearchResource.ListRequest.OrderEnum)Enum.Parse(typeof(SearchResource.ListRequest.OrderEnum),ArguedValue);
            }
            return CurrentOrder;
        }


        private SearchResource.ListRequest.VideoDurationEnum _duration;

        private SearchResource.ListRequest.VideoDurationEnum CurrentDuration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        private SearchResource.ListRequest.VideoDurationEnum SetDurationSetting(Dictionary<string,string> arg)
        {
            if (!arg.ContainsKey("VideoDuration") || arg["VideoDuration"] == "null")
            {
                CurrentDuration = SearchResource.ListRequest.VideoDurationEnum.Any;
            } else {
                string ArguedValue = arg["VideoDuration"];
                CurrentDuration = (SearchResource.ListRequest.VideoDurationEnum)Enum.Parse(typeof(SearchResource.ListRequest.VideoDurationEnum),ArguedValue);
            }
            return CurrentDuration;
        }
    }
}