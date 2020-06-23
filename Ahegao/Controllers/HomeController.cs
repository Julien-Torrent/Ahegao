using Ahegao.Data;
using Ahegao.Models;
using Ahegao.SitesParsers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Ahegao.Controllers
{
    public class HomeController : Controller
    {
        static readonly HttpClient client = new HttpClient();

        private readonly ILogger<HomeController> _logger;
        private readonly SiteContext _context;

        public HomeController(ILogger<HomeController> logger, SiteContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(new SiteViewModel()
            {
                Sites = await _context.GetAllAsync()
            }); 
        }

        [HttpPost]
        public async Task<IActionResult> Index(SiteViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Sites = await _context.GetAllAsync();
                model.ToDownload = model.ToDownload.Trim('/');
                var siteName = model.Sites.Where(x => x.Id == model.SiteId).First().Name;

                var filesContext = new FilesContext(siteName);
                if(await filesContext.IsDoujinDownloadedAsync(model.ToDownload))
                {
                    return GetFileForDownload(siteName, model.ToDownload);
                }

                try
                {
                    var siteType = _context.IdToType(model.SiteId);

                    var site = (ISite)Activator.CreateInstance(siteType);

                    var url = site.GetDownloadUrl(model.Sites.Where(x => x.Id == model.SiteId).First().Url, model.ToDownload);
                    var response = await client.GetStringAsync(url);

                    var genericParser = Type.GetType("Ahegao.Models.HentaiParser`1");
                    var parser = genericParser.MakeGenericType(new Type[]{ siteType });
                    var p = (IParser)Activator.CreateInstance(parser, new string[]{ response, model.ToDownload, model.Sites.Where(x => x.Id == model.SiteId).First().Name });

                    await p.DownloadImages();
                    await p.GeneratePdf();

                    return GetFileForDownload(siteName, model.ToDownload);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Exception Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }

            return View(model);
        }

        private FileContentResult GetFileForDownload(string siteName, string album)
        {
            ContentDisposition cd = new ContentDisposition
            {
                FileName = $"{album}.pdf",
                Inline = false  // false = prompt the user for downloading;  true = browser to try to show the file inline
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());

            return File(System.IO.File.ReadAllBytes($"/app/downloads/{siteName}/{album}.pdf"), MediaTypeNames.Application.Pdf);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
