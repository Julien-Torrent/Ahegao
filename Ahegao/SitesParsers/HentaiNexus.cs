using Ahegao.SitesParsers.Interfaces;
using AngleSharp.Html.Dom;
using System.Collections.Generic;
using System.Linq;

namespace Ahegao.SitesParsers
{
    public class HentaiNexus : ISite
    {
        public string GetDownloadUrl(string siteUrl, string album)
        {
            return siteUrl + (album[^1] != '/' ? album : album.Remove(album.Length - 1)) + "/001";
        }

        public List<string> GetPagesUrls(IHtmlDocument document)
        {
            var urls = document.GetElementsByTagName("script").Where(x => x.TextContent.Contains("images.hentainexus.com")).FirstOrDefault();
            var all = new List<string>();

            foreach (string s in urls.TextContent.Split("\""))
            {
                if (s.StartsWith("https:"))
                {
                    all.Add(s.Replace("\\", ""));
                }
            }

            return all;
        }

        public string RenameFile(string filename)
        {
            return filename.Split("/").Last();
        }
    }
}
