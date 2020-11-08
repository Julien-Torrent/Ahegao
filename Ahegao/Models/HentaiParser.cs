using Ahegao.Data;
using Ahegao.SitesParsers.Interfaces;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ahegao.Models
{
    /// <summary>
    /// Implementation of IPaser, allow to download images and generate PDFs from various sites
    /// </summary>
    /// <typeparam name="T">ISite, with new() -> The methods corresponding to the Site to download from</typeparam>
    public class HentaiParser<T> : IParser where T : ISite, new()
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly IHtmlDocument _document;
        private readonly string _subfolder;
        private readonly string _albumName;
        private readonly FilesContext _filesContext;

        /// <summary>
        /// Create a new HentaiPaser from the page and store in sitename/subfolder
        /// </summary>
        /// <param name="html">The downloaded page content as string</param>
        /// <param name="subfolder">The name or number of the doujin to download</param>
        /// <param name="siteName">Name of the site, comming from the Site Context</param>
        public HentaiParser(string html, string subfolder, string siteName, string basePath)
        {
            _albumName = subfolder;
            _document = new HtmlParser().ParseDocumentAsync(html).Result;
            _subfolder = Path.Combine(basePath, "downloads", siteName, subfolder);
            _filesContext = new FilesContext(siteName, basePath);
        }

        public async Task DownloadImages()
        {
            Directory.CreateDirectory(_subfolder);

            var tasks = new List<Task>();

            foreach (var url in new T().GetPagesUrls(_document))
            {
                tasks.Add(Task.Run(async () =>
                {
                    var image = await (await client.GetAsync(url)).Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync($"{_subfolder}/{new T().RenameFile(url)}", image);
                }));
            }

            await Task.WhenAll(tasks);
        }

        public async Task GeneratePdf()
        {
            using var stream = new FileStream($"{_subfolder}.pdf", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            PdfDocument document = new PdfDocument();

            var files = Directory.GetFiles(_subfolder, "*.*").Select(f => f.Split(Path.DirectorySeparatorChar).Last()).ToList();
            foreach (var file in files.OrderBy(z => int.Parse(z.Split(".").First())))
            {
                // Add a page
                PdfPage page = document.Pages.Add();

                // Load the image from the disk
                using var img = new FileStream(Path.Combine(_subfolder, file), FileMode.Open, FileAccess.Read);
                PdfBitmap image = new PdfBitmap(img);

                // Draw the image with image bounds
                SizeF pageSize = page.GetClientSize();
                page.Graphics.DrawImage(image, new RectangleF(0, 0, pageSize.Width, pageSize.Height));
            }

            // Save the document
            document.Save(stream);
            document.Close(true);

            // Mark the file as downloaded & remove the download folder
            await _filesContext.AddDownloadedAsync(_albumName);
            Directory.Delete(_subfolder, true);
        }
    }
}
