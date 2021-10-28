using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InspectorCaymanSUpdater
{
    class SoftwereUpdateLoader: IUpdateLoader
    {
        private const string _inspectorSiteDomen = @"https://www.rd-inspector.ru/";
        private const string _inspectorCaymanSSoftwereUpdatePageUrl = @"support/inspector-cayman-s/";
        private const string _updateFileDownloadPath = @".\SoftwereUpdate";
        public void LoadUpdate(string targetDirectoryName)
        {
            IConfiguration configuration = Configuration.Default;
            IBrowsingContext browsingContext = new BrowsingContext(configuration);

            string updateFileUrl = GetSoftwereUpdateFileUrl(browsingContext, _inspectorSiteDomen + _inspectorCaymanSSoftwereUpdatePageUrl);

            var webClient = new WebClient();
            webClient.DownloadFile(updateFileUrl, _updateFileDownloadPath);

            ZipFile.ExtractToDirectory(_updateFileDownloadPath, targetDirectoryName);
            File.Delete(_updateFileDownloadPath);
        }

        private string GetSoftwereUpdateFileUrl(IBrowsingContext browsingContext, string dbUpdatePageUrl)
        {
            string updateFileUrl;
            string webPage = GetWebPage(dbUpdatePageUrl);
            using (IDocument document = browsingContext.OpenAsync(req => req.Content(webPage)).Result)
            {
                updateFileUrl = document.QuerySelectorAll("a.url_file_btn")
                    .Where(element => element.TextContent.Contains("Скачать"))
                    .Select(element => element.GetAttribute("href"))
                    .FirstOrDefault();
            }

            return updateFileUrl;
        }

        private string GetWebPage(string pageUrl)
        {
            string webPage;
            WebRequest request = WebRequest.Create(pageUrl);
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    webPage = reader.ReadToEnd();
                }
            }
            return webPage;
        }

    }
}
