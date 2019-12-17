using System.Threading.Tasks;

namespace Ahegao.SitesParsers.Interfaces
{
    interface IParser
    {
        public void DownloadImages();
        public void GeneratePdf();
    }
}
