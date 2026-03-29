using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class Submission
    {
        public static string xmlURL = "https://zchalmers.github.io/CSE445-Assignment4/NationalParks.xml";
        public static string xmlErrorURL = "https://zchalmers.github.io/CSE445-Assignment4/NationalParksErrors.xml";
        public static string xsdURL = "https://zchalmers.github.io/CSE445-Assignment4/NationalParks.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            StringBuilder errors = new StringBuilder();
            string xml = DownloadContent(xmlUrl);
            string xsd = DownloadContent(xsdUrl);

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
            }
            catch (XmlException ex)
            {
                return ex.Message;
            }

            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(null, XmlReader.Create(new StringReader(xsd)));
            schemas.Compile();

            doc.Schemas = schemas;
            doc.Validate((sender, e) => errors.AppendLine(e.Message));

            if (errors.Length > 0)
                return errors.ToString().TrimEnd();

            return "No errors are found";
        }

        // Q2.2
        public static string Xml2Json(string xmlUrl)
        {
            XmlDocument doc = new XmlDocument();
            string download = DownloadContent(xmlUrl);
            doc.LoadXml(download);
            return JsonConvert.SerializeXmlNode(doc.DocumentElement, Newtonsoft.Json.Formatting.None, omitRootObject: false);
        }

        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}
