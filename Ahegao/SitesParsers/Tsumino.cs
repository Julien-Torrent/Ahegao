using Ahegao.SitesParsers.Interfaces;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ahegao.SitesParsers
{
    // Problems with check not a bot
    public class Tsumino : ISite
    {
        public string GetDownloadUrl(string siteUrl, string album)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<string>> GetPagesUrls(string html)
        {
            IHtmlDocument document = await new HtmlParser().ParseDocumentAsync(html);
            return new List<string>(); // _document.QuerySelectorAll(".gallerythumb > img").Select(x => x.GetAttribute("data-src")).ToList();
        }

        public List<string> GetPagesUrls(IHtmlDocument document)
        {
            throw new System.NotImplementedException();
        }

        public string RenameFile(string filename)
        {
            throw new System.NotImplementedException();
        }
    }
}
