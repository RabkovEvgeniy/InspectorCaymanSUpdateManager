using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using AngleSharp;
using AngleSharp.Dom;

namespace InspectorCaymanSUpdater
{
    class DbUpdateLoader : IUpdateLoader
    {
        private const string _inspectorSiteDomen = @"https://www.rd-inspector.ru/";
        private const string _inspectorSupportPageUrl = "support/";
        private const string _inspectorCaymanSDbUpdatePattern = "Inspector Cayman S (обновление базы данных)";
        private const string _updateFileDownloadPath = @".\DbUpdate";
        public void LoadUpdate(string targetDirectoryName)
        {
            IConfiguration configuration = Configuration.Default;
            IBrowsingContext browsingContext = new BrowsingContext(configuration);

            string dbUpdatePageUrl = GetDbUpdatePageUrl(browsingContext, _inspectorSiteDomen + _inspectorSupportPageUrl);
            string updateFileUrl = GetDbUpdateFileUrl(browsingContext, _inspectorSiteDomen + dbUpdatePageUrl);

            var webClient = new WebClient();
            webClient.DownloadFile(updateFileUrl, _updateFileDownloadPath);

            ZipFile.ExtractToDirectory(_updateFileDownloadPath, targetDirectoryName);
            File.Delete(_updateFileDownloadPath);
        }

        private string GetDbUpdatePageUrl(IBrowsingContext browsingContext,string supportPageUrl)
        {
            string dbUpdatePageUrl;
            string webPage = GetWebPage(supportPageUrl);
            using (IDocument document = browsingContext.OpenAsync(req => req.Content(webPage)).Result)
            {
                dbUpdatePageUrl = document.QuerySelectorAll("a")
                    .Where(element => element.ParentElement.ParentElement.TextContent.Contains(_inspectorCaymanSDbUpdatePattern))
                    .Select(element => element.GetAttribute("href"))
                    .FirstOrDefault();
            }

            return dbUpdatePageUrl;
        }

        private string GetDbUpdateFileUrl(IBrowsingContext browsingContext, string dbUpdatePageUrl)
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
