using System.Threading.Tasks;

namespace Ahegao.SitesParsers.Interfaces
{
    /// <summary>
    /// Represents a Parser able to download images from ISite and Generate a PDF from thoses images
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Download all the images for the current (ISite, album) pair
        /// </summary>
        /// <returns>An awaitable Task (void), when all the images have been downloaded</returns>
        public Task DownloadImages();

        /// <summary>
        /// Generate the pdf from all the downloaded images
        /// Marks the file as downloaded in the corresponding SQLite database
        /// </summary>
        /// <returns>An awaitable Task (void), when the PDF file has been generated and saved</returns>
        public Task GeneratePdf();
    }
}
