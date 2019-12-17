using Ahegao.SitesParsers.Interfaces;
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

        public void DownloadImages()
        {
            Directory.CreateDirectory(_subfolder);

            Parallel.ForEach(new T().GetPagesUrls(_document), async url =>
            {
                var image = await (await client.GetAsync(url)).Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync($"{_subfolder}/{new T().RenameFile(url)}", image);
            });
        }

        public async void GeneratePdf()
        {
            var html = new StringBuilder("<html><body>");

            var files = Directory.GetFiles(_subfolder,"*.*").Where(x => x.EndsWith(".png") | x.EndsWith(".jpg")).Select(f => f.Split(Path.DirectorySeparatorChar).Last()).ToArray();

            foreach (var file in files.OrderBy(z => int.Parse(z.Split(".").First())))
            {
                html.Append($"<img width=\"100%\" src=\"{file.Split('/').Last()}\"/>");
            }

            html.Append("<body></html>");

            await File.WriteAllTextAsync($"{_subfolder}/template.html", html.ToString());

            var PDF = await new HtmlToPdf().RenderHTMLFileAsPdfAsync($"{_subfolder}/template.html");
            PDF.SaveAs($"{_subfolder}.pdf");
        }
    }
}
