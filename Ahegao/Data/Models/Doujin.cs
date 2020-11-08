using System.ComponentModel.DataAnnotations;

namespace Ahegao.Data.Models
{
    /// <summary>
    /// Represents a downloaded doujin
    /// </summary>
    public class Doujin
    {
        public enum States { Downloading, Downloaded, Error }

        /// <summary>
        /// Id for database storage
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Name of the downloaded doujin
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// State of the download
        /// </summary>
        public States State { get; set; }
    }
}
