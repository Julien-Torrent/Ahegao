using Ahegao.Data;
using Ahegao.Models;
using Ahegao.SitesParsers;
using Ahegao.SitesParsers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ahegao.Controllers
{
    public class HomeController : Controller
    {
        static readonly HttpClient client = new HttpClient();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(new SiteViewModel()
            {
                Sites = await new SiteContext().GetAllAsync()
            }); 
        }

        [HttpPost]
        public async Task<IActionResult> Index(SiteViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Sites = await new SiteContext().GetAllAsync();

                try
                {
                    var context = new SiteContext();
                    var siteType = context.IdToType(model.SiteId);

                    var site = (ISite)Activator.CreateInstance(siteType);

                    var url = site.GetDownloadUrl(model.Sites.Where(x => x.Id == model.SiteId).First().Url, model.ToDownload);
                    var response = await client.GetStringAsync(url);

                    var genericParser = Type.GetType("Ahegao.Models.HentaiParser`1");
                    var parser = genericParser.MakeGenericType(new Type[]{ siteType });
                    var p = (IParser)Activator.CreateInstance(parser, new string[]{ response, model.ToDownload, model.Sites.Where(x => x.Id == model.SiteId).First().Name  });

                    await p.DownloadImagesAsync();
                    p.GeneratePdf();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Exception Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }

            return View(model);
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
