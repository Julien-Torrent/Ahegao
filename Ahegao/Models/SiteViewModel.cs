using System.Collections.Generic;

namespace Ahegao.Models
{
    public class SiteViewModel
    {
        public List<Site> Sites { get; set; }
        public string ToDownload { get; set; }
        public int SiteId { get; set; }
    }
}
