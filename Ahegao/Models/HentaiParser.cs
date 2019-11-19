﻿using Ahegao.SitesParsers.Interfaces;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using IronPdf;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ahegao.Models
{
    public class HentaiParser<T> : IParser where T : ISite, new()
    {
        private static readonly HttpClient client = new HttpClient();

        private IHtmlDocument _document;
        private readonly string _subfolder;

        public HentaiParser(string html, string subfolder, string siteName)
        {
            Task.Run(async () => _document = await new HtmlParser().ParseDocumentAsync(html)).Wait();
            _subfolder = $"downloads/{siteName}/{subfolder}";
        }

        public async Task DownloadImagesAsync()
        {
            foreach(var e in new T().GetPagesUrls(_document))
            {
                var image = await (await client.GetAsync(e)).Content.ReadAsByteArrayAsync();

                Directory.CreateDirectory(_subfolder);
                await File.WriteAllBytesAsync($"{_subfolder}/{new T().RenameFile(e)}", image);
            }
        }

        public async void GeneratePdf()
        {
            var html = new StringBuilder("<html><body>");

            var files = Directory.GetFiles(_subfolder,"*.*").Where(x => x.EndsWith(".png") | x.EndsWith(".jpg")).Select(f => f.Split('/').Last()).ToArray();

            foreach (var file in files.OrderBy(z => int.Parse(z.Split(".").First())))
            {
                html.Append($"<img width=\"100%\" src=\"{file.Split('/').Last()}\"/>");
            }

            html.Append("<body></html>");

            await File.WriteAllTextAsync($"{_subfolder}/template.html", html.ToString());

            var Renderer = new HtmlToPdf();
            var PDF = await Renderer.RenderHTMLFileAsPdfAsync($"{_subfolder}/template.html");
            PDF.SaveAs($"{_subfolder}.pdf");
        }
    }
}