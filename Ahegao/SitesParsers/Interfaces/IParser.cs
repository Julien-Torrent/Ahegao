using System.Threading.Tasks;

namespace Ahegao.SitesParsers.Interfaces
{
    interface IParser
    {
        public Task DownloadImagesAsync();
        public void GeneratePdf();
    }
}
