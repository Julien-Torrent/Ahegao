using System.Threading.Tasks;

namespace Ahegao.SitesParsers.Interfaces
{
    interface IParser
    {
        public Task DownloadImages();
        public Task GeneratePdf();
    }
}
