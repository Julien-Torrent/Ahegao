using Ahegao.SitesParsers.Interfaces;
using AngleSharp.Html.Dom;
using System.Collections.Generic;
using System.Linq;

namespace Ahegao.SitesParsers
{
    public class Nhentai : ISite
    {
        public string GetDownloadUrl(string siteUrl, string album)
        {
            return siteUrl + album;
        }

        public List<string> GetPagesUrls(IHtmlDocument document)
        {
            return document.QuerySelectorAll(".gallerythumb > img").Select(x => x.GetAttribute("data-src")).Select(x => x.Remove(x.Length - 5, 1).Replace("/t", "/i")) .ToList();
        }

        public string RenameFile(string filename)
        {
            return new string(filename.Split("/").Last().Where(c => char.IsDigit(c)).ToArray()) + "." + filename.Split(".").Last();
        }
    }
}
