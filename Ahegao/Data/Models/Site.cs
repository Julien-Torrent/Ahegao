using System.ComponentModel.DataAnnotations;

namespace Ahegao.Models
{
    public class Site
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Regex { get; set; }
    }
}
