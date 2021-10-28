using AngleSharp;
using AngleSharp.Dom;
using InspectorCaymanSUpdater.Services;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Windows;

namespace InspectorCaymanSUpdater
{
    class SoftwereUpdateLoader : IUpdateLoader
    {
        private const string _inspectorSiteDomen = @"https://www.rd-inspector.ru/";
        private const string _inspectorCaymanSSoftwereUpdatePageUrl = @"support/inspector-cayman-s/";
        private const string _updateFileDownloadPath = @".\SoftwereUpdate";
        public void LoadUpdate(string targetDirectoryName, INotifyChangedLogger logger)
        {
            try
            {
                IConfiguration configuration = Configuration.Default;
                using (IBrowsingContext browsingContext = new BrowsingContext(configuration))
                {

                    logger.LogInformation("Получаю URL файла обновления ПО");
                    string updateFileUrl = GetSoftwereUpdateFileUrl(browsingContext, _inspectorSiteDomen + _inspectorCaymanSSoftwereUpdatePageUrl);

                    logger.LogInformation("Загружаю файл обновлений ПО");
                    using (var webClient = new WebClient())
                    {
                        webClient.DownloadFile(updateFileUrl, _updateFileDownloadPath);
                    }

                    logger.LogInformation($"Распаковываю файл обновлений ПО в {targetDirectoryName}");
                    ZipFile.ExtractToDirectory(_updateFileDownloadPath, targetDirectoryName);

                    logger.LogInformation("Удаляю временные файлы");
                    File.Delete(_updateFileDownloadPath);

                    logger.LogInformation("Операция прошла успешно");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Произошла ошибка: {ex.Message}");
                MessageBox.Show("Произошла ошибка за доп. сведениями обратитесь в поддержку", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static string GetSoftwereUpdateFileUrl(IBrowsingContext browsingContext, string dbUpdatePageUrl)
        {
            string updateFileUrl;
            string webPage = Parsing.GetWebPage(dbUpdatePageUrl);
            using (IDocument document = browsingContext.OpenAsync(req => req.Content(webPage)).Result)
            {
                updateFileUrl = document.QuerySelectorAll("a.url_file_btn")
                    .Where(element => element.TextContent.Contains("Скачать"))
                    .Select(element => element.GetAttribute("href"))
                    .FirstOrDefault();
            }

            return updateFileUrl;
        }


    }
}
