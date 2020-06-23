using System.ComponentModel.DataAnnotations;

namespace Ahegao.Models
{
    /// <summary>
    /// Represent a site with an Id, Name, and base URL
    /// </summary>
    public class Site
    {
        /// <summary>
        /// The Id of the site, auto generated
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// Firendly name of the site
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Base URL for downloads
        /// </summary>
        public string Url { get; set; }
    }
}
