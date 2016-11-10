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

        private SearchResource.ListRequest.VideoDefinitionEnum _definition;

        public SearchResource.ListRequest.VideoDefinitionEnum CurrentDefinition
        {
            get { return _definition; }
            set { _definition = value; }
        }

        public SearchResource.ListRequest.VideoDefinitionEnum SetDefinitionSetting(Dictionary<string,string> arg)
        {
            if (!arg.ContainsKey("VideoDefinition") || arg["VideoDefinition"] == "null")
            {
                CurrentDefinition = SearchResource.ListRequest.VideoDefinitionEnum.Any;
            } else {
                string ArguedDefinition = arg["VideoDefinition"];
                switch(ArguedDefinition){
                    case ("high"):
                        CurrentDefinition = SearchResource.ListRequest.VideoDefinitionEnum.High;
                        break;
                    case ("standard"):
                        CurrentDefinition = SearchResource.ListRequest.VideoDefinitionEnum.Standard;
                        break;
                    default:
                        CurrentDefinition = SearchResource.ListRequest.VideoDefinitionEnum.Any;
                        break;                                                                                              
                }
            }
            return CurrentDefinition;
        }


        private SearchResource.ListRequest.VideoDimensionEnum _dimension;

        public SearchResource.ListRequest.VideoDimensionEnum CurrentDimension
        {
            get { return _dimension; }
            set { _dimension = value; }
        }

        public SearchResource.ListRequest.VideoDimensionEnum SetDimensionSetting(Dictionary<string,string> arg)
        {
            if (!arg.ContainsKey("VideoDimension") || arg["VideoDimension"] == "null")
            {
                CurrentDimension = SearchResource.ListRequest.VideoDimensionEnum.Any;
            } else {
                string ArguedValue = arg["VideoDimension"];
                switch(ArguedValue){
                    case ("2D"):
                        CurrentDimension = SearchResource.ListRequest.VideoDimensionEnum.Value2d;
                        break;
                    case ("3D"):
                        CurrentDimension = SearchResource.ListRequest.VideoDimensionEnum.Value3d;
                        break;
                    default:
                        CurrentDimension = SearchResource.ListRequest.VideoDimensionEnum.Any;
                        break;                                                                                              
                }
            }
            return CurrentDimension;
        }


        private SearchResource.ListRequest.OrderEnum _order;

        public SearchResource.ListRequest.OrderEnum CurrentOrder
        {
            get { return _order; }
            set { _order = value; }
        }

        public SearchResource.ListRequest.OrderEnum SetOrderSetting(Dictionary<string,string> arg)
        {
            if (!arg.ContainsKey("Order") || arg["Order"] == "null")
            {
                CurrentOrder = SearchResource.ListRequest.OrderEnum.Relevance;
            } else {
                string ArguedValue = arg["Order"];
                switch(ArguedValue){
                    case ("Relevance"):
                        CurrentOrder = SearchResource.ListRequest.OrderEnum.Relevance;
                        break;
                    case ("Date"):
                        CurrentOrder = SearchResource.ListRequest.OrderEnum.Date;
                        break;
                    case ("Rating"):
                        CurrentOrder = SearchResource.ListRequest.OrderEnum.Rating;
                        break;
                    case ("Title"):
                        CurrentOrder = SearchResource.ListRequest.OrderEnum.Title;
                        break;
                    case ("ViewCount"):
                        CurrentOrder = SearchResource.ListRequest.OrderEnum.ViewCount;
                        break;
                    default:
                        CurrentOrder = SearchResource.ListRequest.OrderEnum.Relevance;
                        break;                                                                                              
                }
            }
            return CurrentOrder;
        }


        private SearchResource.ListRequest.VideoDurationEnum _duration;

        public SearchResource.ListRequest.VideoDurationEnum CurrentDuration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public SearchResource.ListRequest.VideoDurationEnum SetDurationSetting(Dictionary<string,string> arg)
        {
            if (!arg.ContainsKey("VideoDuration") || arg["VideoDuration"] == "null")
            {
                CurrentDuration = SearchResource.ListRequest.VideoDurationEnum.Any;
            } else {
                string ArguedValue = arg["VideoDuration"];
                switch(ArguedValue){
                    case ("long"):
                        CurrentDuration = SearchResource.ListRequest.VideoDurationEnum.Long__;
                        break;
                    case ("medium"):
                        CurrentDuration = SearchResource.ListRequest.VideoDurationEnum.Medium;
                        break;
                    case ("short"):
                        CurrentDuration = SearchResource.ListRequest.VideoDurationEnum.Short__;
                        break;
                    default:
                        CurrentDuration = SearchResource.ListRequest.VideoDurationEnum.Any;
                        break;                                                                                              
                }
            }
            return CurrentDuration;
        }
    }
}