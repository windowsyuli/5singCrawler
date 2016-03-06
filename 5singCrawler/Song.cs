using System;
using System.Collections.Generic;
using System.Linq;

namespace _5singCrawler
{
    public class Song
    {
        string _url = "";
        public string DownloadUrl = "";
        public string SongName = "";

        public Song(string url)
        {
            _url = url;
        }

        public bool FindTicket()
        {
            //content
            string content = Helper.HttpDownloads(_url);
            if (content == "")
            {
                Helper.Log(_url, "Download content failed.");
                return false;
            }

            //ticket
            List<string> tmp = Helper.MatchContent("\"ticket\": \"", "\"", content);
            if (tmp.Count == 0)
            {
                Helper.Log(_url, "Can not find ticket.");
                return false;
            }
            string ticket = tmp[0];

            //name
            string songName = "";
            try
            {
                tmp = Helper.MatchContent("<title>", "<", content);
                tmp = tmp[0].Split('-').ToList();
                songName = tmp[1] + "-" + tmp[0];
            }
            catch (Exception)
            {

            }

            //decode
            string result = Helper.Base64(ticket);
            if (result == "")
            {
                Helper.Log(_url, "Can not decode base64.");
                return false;
            }

            //find downlaodUrl
            tmp = Helper.MatchContent("\"file\":\"", "\"", result);
            if (tmp.Count == 0)
            {
                Helper.Log(_url, "Can not find download url.");
                return false;
            }
            DownloadUrl = tmp[0].Replace("\\/", "/");

            if(songName == "")
            {
                songName = Helper.MatchContent("com/", ".mp3", DownloadUrl)[0];
            }
            SongName = songName;
            return true;
        }
    }
}
