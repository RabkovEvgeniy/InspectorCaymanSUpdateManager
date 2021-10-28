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
    class DbUpdateLoader : IUpdateLoader
    {
        private const string _inspectorSiteDomen = @"https://www.rd-inspector.ru/";
        private const string _inspectorSupportPageUrl = "support/";
        private const string _inspectorCaymanSDbUpdatePattern = "Inspector Cayman S (обновление базы данных)";
        private const string _updateFileDownloadPath = @".\DbUpdate";
        public void LoadUpdate(string targetDirectoryName, INotifyChangedLogger logger)
        {
            try
            {
                IConfiguration configuration = Configuration.Default;
                using (IBrowsingContext browsingContext = new BrowsingContext(configuration))
                {
                    logger.LogInformation("Получаю URL страницы обновлений БД");
                    string dbUpdatePageUrl = GetDbUpdatePageUrl(browsingContext, _inspectorSiteDomen + _inspectorSupportPageUrl);

                    logger.LogInformation("Получаю URL файла обновления БД");
                    string updateFileUrl = GetDbUpdateFileUrl(browsingContext, _inspectorSiteDomen + dbUpdatePageUrl);

                    logger.LogInformation("Загружаю файл обновлений БД");
                    using (var webClient = new WebClient())
                    {
                        webClient.DownloadFile(updateFileUrl, _updateFileDownloadPath);
                    }

                    logger.LogInformation($"Распаковываю полученный архив в {targetDirectoryName}");
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

        private static string GetDbUpdatePageUrl(IBrowsingContext browsingContext, string supportPageUrl)
        {
            string dbUpdatePageUrl;
            string webPage = Parsing.GetWebPage(supportPageUrl);
            using (IDocument document = browsingContext.OpenAsync(req => req.Content(webPage)).Result)
            {
                dbUpdatePageUrl = document.QuerySelectorAll("a")
                    .Where(element => element.ParentElement.ParentElement.TextContent.Contains(_inspectorCaymanSDbUpdatePattern))
                    .Select(element => element.GetAttribute("href"))
                    .FirstOrDefault();
            }

            return dbUpdatePageUrl;
        }

        private static string GetDbUpdateFileUrl(IBrowsingContext browsingContext, string dbUpdatePageUrl)
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
