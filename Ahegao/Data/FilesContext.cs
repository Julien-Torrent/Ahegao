using Ahegao.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
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

        private DbSet<Doujin> Doujins { get; set; }

        /// <summary>
        /// Create a new Context, with a database named files.db in {basepath}/downloads/{siteName}/files.db
        /// </summary>
        /// <param name="siteName">Name of the site for the database</param>
        public FilesContext(string siteName, string basePath)
        {
            _databasePath = Path.Combine(basePath, "downloads", siteName, "files.db");
            if(!Directory.Exists(Path.Combine(basePath, "downloads", siteName)))
            {
                Directory.CreateDirectory(Path.Combine(basePath, "downloads", siteName));
            }
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Doujin>().HasIndex(s => s.Name).IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");
        }

        /// <summary>
        /// Check if the name is already in the database
        /// </summary>
        /// <param name="name">Name of the doujin to check, should be on the SiteName site</param>
        /// <returns>True if already downloaded, then you need to check for name.pdf</returns>
        public async Task<bool> IsDoujinDownloadedAsync(string name)
        {
            return await Doujins.AnyAsync(x => x.Name == name && x.State == Doujin.States.Downloaded);
        }

        public async Task<bool> AddDownloadingAsync(string name)
        {
            await Doujins.AddAsync(new Doujin() { Name = name, State = Doujin.States.Downloading });

            try
            {
                await SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        /// <summary>
        /// Mark the name as downloaded doujin, for the current SiteName
        /// </summary>
        /// <param name="name">Name of the doujin to mark as downloaded</param>
        /// <returns>Awaitable Task, returns when inserted</returns>
        public async Task AddDownloadedAsync(string name)
        {
            var doujin = await Doujins.Where(x => x.Name == name).FirstAsync();
            doujin.State = Doujin.States.Downloaded;
            Doujins.Update(doujin);
            await SaveChangesAsync();
        }
    }
}
