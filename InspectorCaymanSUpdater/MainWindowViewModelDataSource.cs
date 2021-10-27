using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AngleSharp;
using AngleSharp.Dom;

namespace InspectorCaymanSUpdater
{
    class MainWindowViewModelDataSource : IMainWindowViewModelDataSource
    {
        private const string _inspectorSupportPageUrl = "https://www.rd-inspector.ru/support/";
        private const string _inspectorCaymanSDbUpdateDataPattern= "Inspector Cayman S (обновление базы данных)";
        private const string _inspectorCaymanSSoftwereUpdateDataPattern= "Inspector Cayman S (обновление ПО)";
        public string GetLastDbUpdateDate()
        {
            string webPage;
            WebRequest request = WebRequest.Create(_inspectorSupportPageUrl);
            WebResponse response = request.GetResponse();

            using (Stream stream = response.GetResponseStream()) 
            {
                using(StreamReader reader = new StreamReader(stream)) 
                {
                    webPage = reader.ReadToEnd();
                }
            }

            IConfiguration config = Configuration.Default;
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument document = context.OpenAsync(req => req.Content(webPage)).Result;

            string inspectorCaymanSUpdateData = document.QuerySelectorAll("span")
                .Select(element => element.TextContent)
                .Where(text => text.Contains(_inspectorCaymanSDbUpdateDataPattern))
                .FirstOrDefault();

            return inspectorCaymanSUpdateData.Split(" ")[^1]; 
        }

        public string GetLastSoftwereUpdateDate()
        {
            string webPage;
            WebRequest request = WebRequest.Create(_inspectorSupportPageUrl);
            WebResponse response = request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    webPage = reader.ReadToEnd();
                }
            }

            IConfiguration config = Configuration.Default;
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument document = context.OpenAsync(req => req.Content(webPage)).Result;

            string inspectorCaymanSUpdateData = document.QuerySelectorAll("span")
                .Select(element => element.TextContent)
                .Where(text => text.Contains(_inspectorCaymanSSoftwereUpdateDataPattern))
                .FirstOrDefault();

            return inspectorCaymanSUpdateData.Split(" ")[^1];
        }
    }
}
