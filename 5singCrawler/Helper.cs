using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace _5singCrawler
{
    class Helper
    {
        public static string HttpDownloads(string url, string Encode = "utf-8")
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Timeout = 10000;
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
                using (StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding(Encode)))
                    return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static List<string> MatchContent(string pattern, string content)
        {
            List<string> result = new List<string>();
            MatchCollection match = new Regex(pattern, RegexOptions.Singleline).Matches(content);
            foreach (Match t in match)
                result.Add(t.Value);
            return result;
        }

        public static List<string> MatchContent(string before, string end, string content)
        {
            string pattern = "(?<=" + before + ").+?(?=" + end + ")";
            return MatchContent(pattern, content);
        }

        public static void Log(string url, string message)
        {
            Console.WriteLine("url: " + url);
            Console.WriteLine("message: " + message);
            using (StreamWriter sr = new StreamWriter("log.txt", true))
            {
                sr.WriteLine("url:  " + url);
                sr.WriteLine("message: " + message);
            }
        }

        public static bool WebDownload(string url, string name)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.DownloadFile(url, name);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string Base64(string str)
        {
            try
            {
                return Encoding.Default.GetString(Convert.FromBase64String(str));
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
