using Ahegao.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ahegao.Data
{
    /// <summary>
    /// Class to access SQLite databases to keep track of already downloaded doujins
    /// Doesn't take into account if the file as been deleted (files shouldn't be deleted)
    /// </summary>
    public class FilesContext : DbContext
    {
        private readonly string _databasePath;

        public DbSet<Doujin> Downloaded { get; set; }

        /// <summary>
        /// Create a new Context, with a database named files.db in /app/downloads/siteName
        /// </summary>
        /// <param name="siteName">Name of the site for the database</param>
        public FilesContext(string siteName)
        {
            _databasePath = $"/app/downloads/{siteName}/files.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");
        }

        /// <summary>
        /// Check if the name is already in the database
        /// </summary>
        /// <param name="name">Name of the doujin to check, should be on the SiteName site</param>
        /// <returns>True if already downloaded, then you need to check for name.pdf</returns>
        public async Task<bool> IsDoujinDownloadedAsync(string name)
        {
            await Database.EnsureCreatedAsync();
            return await Downloaded.AnyAsync(x => x.Name == name);
        }

        /// <summary>
        /// Mark the name as downloaded doujin, for the current SiteName
        /// </summary>
        /// <param name="name">Name of the doujin to mark as downloaded</param>
        /// <returns>Awaitable Task, returns when inserted</returns>
        public async Task AddDownloadedAsync(string name)
        {
            await Downloaded.AddAsync(new Doujin()
            {
                Name = name
            });
            await SaveChangesAsync();
        }
    }
}
