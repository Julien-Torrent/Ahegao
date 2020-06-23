using Ahegao.SitesParsers.Interfaces;
using AngleSharp.Html.Dom;
using Flurl;
using System.Collections.Generic;
using System.Linq;

namespace Ahegao.SitesParsers
{
    public class Hentai2Read : ISite
    {
        public string GetDownloadUrl(string siteUrl, string album)
        {
            return siteUrl.AppendPathSegments(album, "1");
        }

        public List<string> GetPagesUrls(IHtmlDocument document)
        {
            var url = document.QuerySelector("#arf-reader").GetAttribute("src").ToString();

            List<string> urls = new List<string>();
            for (int i = 1; i <= document.QuerySelector(".dropdown-menu").ChildElementCount; i++)
            {
                if (url.Split("/").Last()[0] == 'p')
                    urls.Add(url.Replace("p001", "p" + i.ToString("000")));
                else
                    urls.Add(url.Replace("0001", i.ToString("0000")));
            }
            return urls;
        }

        public string RenameFile(string filename)
        {
            return new string(filename.Split("/").Last().Where(c => char.IsDigit(c)).ToArray()) + "." + filename.Split(".").Last();
        }
    }
}
