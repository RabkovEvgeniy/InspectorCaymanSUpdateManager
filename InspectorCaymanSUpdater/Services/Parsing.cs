using System.IO;
using System.Net;

namespace InspectorCaymanSUpdater.Services
{
    public static class Parsing
    {
        public static string GetWebPage(string pageUrl)
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
