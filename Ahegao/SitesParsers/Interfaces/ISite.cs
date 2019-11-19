using AngleSharp.Html.Dom;
using System.Collections.Generic;

namespace Ahegao.SitesParsers.Interfaces
{
    public interface ISite
    {
        public string GetDownloadUrl(string siteUrl, string album);
        public List<string> GetPagesUrls(IHtmlDocument document);
        public string RenameFile(string filename);
    }
}
