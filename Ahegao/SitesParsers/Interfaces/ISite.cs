using AngleSharp.Html.Dom;
using System.Collections.Generic;

namespace Ahegao.SitesParsers.Interfaces
{
    /// <summary>
    /// Represents a site from which we can download doujins
    /// A site can get the first page, get all the images URLs, and rename the images for storage
    /// </summary>
    public interface ISite
    {
        /// <summary>
        /// Get the first page of the doujin for the disred site and album
        /// </summary>
        /// <param name="siteUrl">The url of the site to download from</param>
        /// <param name="album">The doujin to download, format depends of the site</param>
        /// <returns>The URL of the page used to scrap all the images URLs</returns>
        public string GetDownloadUrl(string siteUrl, string album);

        /// <summary>
        /// Extract all the doujin images URLs from the page 
        /// </summary>
        /// <param name="document">The HTML document to extract the urls from</param>
        /// <returns>All the images URLs corresponding to the doujin</returns>
        public List<string> GetPagesUrls(IHtmlDocument document);

        /// <summary>
        /// Take the url of the image and transform it to the name of the file
        /// The name of the file will be [0-9]{1, *}.(jpg | png)
        /// </summary>
        /// <param name="filename">The url of the image to save</param>
        /// <returns>The name of the file to save</returns>
        public string RenameFile(string filename);
    }
}
