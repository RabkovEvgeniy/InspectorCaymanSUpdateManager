using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
