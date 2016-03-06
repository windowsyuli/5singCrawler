using System;
using System.Collections.Generic;
using System.IO;

namespace _5singCrawler
{
    class Program
    {
        static List<string> urlLists = new List<string>();

        static void Main(string[] args)
        {
            string url = "";
            if (args.Length == 1)
            {
                url = args[0];
            }
            else if(args.Length == 0)
            {
                url = Console.ReadLine();
                //url = "http://5sing.kugou.com/yc/2526208.html";
            }
            else
            {

            }
            Search(url);
            Search(url, "#listenContent");
            urlLists.ForEach(u =>
            {
                Song s = new Song(u);
                if (s.FindTicket())
                {
                    Console.WriteLine("Find url: " + s.DownloadUrl);
                    //Process.Start(@"C:\Program Files (x86)\Thunder Network\Thunder\Program\Thunder.exe", " \"" + s.DownloadUrl + "\" [\"" + s.SongName + ".mp3\"]");
                    if (!Directory.Exists("./Download"))
                        Directory.CreateDirectory("./Download");
                    Helper.WebDownload(s.DownloadUrl, "./Download/" + s.SongName + ".mp3");
                    Console.WriteLine("Download complete.");
                }
            });
        }
        static void Search(string url, string exStr = "")
        {
            string content = Helper.HttpDownloads(url);
            string pattern = "(?<=href=\")" + "http://5sing.kugou.com/\\w{2}/\\d{5,12}.html" + "(?=" + exStr + "\")";
            List<string> result = Helper.MatchContent(pattern, content);
            List<string> select = new List<string>();
            result.ForEach(r =>
            {
                if (!select.Contains(r))
                    select.Add(r);
            });
            int count = 0;
            select.ForEach(r =>
            {
                if (!urlLists.Contains(r) && !r.Contains("bz"))
                {
                    urlLists.Add(r);
                    count++;
                }
            });
            if (count == 0)
                return;
            else
                Search(select[select.Count - 1]);
        }
    }
}
