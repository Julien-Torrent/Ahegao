using System.ComponentModel.DataAnnotations;

namespace Ahegao.Data.Models
{
    /// <summary>
    /// Represents a downloaded doujin
    /// </summary>
    public class Doujin
    {
        /// <summary>
        /// Id for database storage
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Name of the downloaded doujin
        /// </summary>
        public string Name { get; set; }
    }
}
