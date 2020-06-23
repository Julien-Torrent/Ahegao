﻿using Ahegao.Data;
using Ahegao.SitesParsers.Interfaces;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using IronPdf;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        private readonly FilesContext _filesContext;

        /// <summary>
        /// Create a new HentaiPaser from the page and store in sitename/subfolder
        /// </summary>
        /// <param name="html">The downloaded page content as string</param>
        /// <param name="subfolder">The name or number of the doujin to download</param>
        /// <param name="siteName">Name of the site, comming from the Site Context</param>
        public HentaiParser(string html, string subfolder, string siteName, string basePath)
        {
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
            var html = new StringBuilder("<html><body>");

            var files = Directory.GetFiles(_subfolder,"*.*").Where(x => x.EndsWith(".png") | x.EndsWith(".jpg")).Select(f => f.Split(Path.DirectorySeparatorChar).Last()).ToArray();

            foreach (var file in files.OrderBy(z => int.Parse(z.Split(".").First())))
            {
                html.Append($"<img width=\"100%\" src=\"{file.Split('/').Last()}\"/>");
            }

            html.Append("<body></html>");

            var PDF = await HtmlToPdf.StaticRenderHtmlAsPdfAsync(html.ToString(), _subfolder);
            PDF.SaveAs($"{_subfolder}.pdf");

            // If not saved, mark the file as downloaded
            if (!await _filesContext.IsDoujinDownloadedAsync(_subfolder.Split(Path.DirectorySeparatorChar).Last()))
            {
                await _filesContext.AddDownloadedAsync(_subfolder.Split(Path.DirectorySeparatorChar).Last());
            }
        }
    }
}
