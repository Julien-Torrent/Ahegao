using Ahegao.SitesParsers.Interfaces;
using AngleSharp.Html.Dom;
using System.Collections.Generic;
using System.Linq;

namespace Ahegao.SitesParsers
{
    public class Tsumino : ISite
    {
        public string GetDownloadUrl(string siteUrl, string album)
        {
            return siteUrl + album;
        }

        public List<string> GetPagesUrls(IHtmlDocument document)
        {
            var url = document.QuerySelector("#image-container").GetAttribute("data-cdn");
            var pagesCount = int.Parse(document.QuerySelector("h1").TextContent.Split(" ").Last());

            var all = new List<string>();

            for(int i = 1; i < pagesCount + 1; i++)
            {
                all.Add(url.Replace("[PAGE]", i.ToString()));
            }

            return all;
        }

        public string RenameFile(string filename)
        {
            return filename.Split('/').Last().Split('?').First() + ".jpg";
        }
    }
}
