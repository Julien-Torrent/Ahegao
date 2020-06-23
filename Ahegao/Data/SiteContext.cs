using Ahegao.Models;
using Ahegao.SitesParsers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ahegao.Data
{
    /// <summary>
    /// Class to access the Sites database
    /// </summary>
    public class SiteContext : DbContext
    {
        public DbSet<Site> Sites { get; set; }

        public SiteContext(DbContextOptions<SiteContext> options) : base(options) { }

        /// <summary>
        /// Get all the sites from the database asynchronously
        /// </summary>
        /// <returns>The list of retieved sites</returns>
        public async Task<List<Site>> GetAllAsync()
        {
            return await Sites.ToListAsync();
        }

        /// <summary>
        /// Convert the site Id to the ISite Type
        /// </summary>
        /// <param name="id">Id of the site</param>
        /// <returns>The Type T to use in the HentaiParser<T></returns>
        public Type IdToType(int id)
        {
            return id switch
            {
                1 => typeof(Nhentai),
                2 => typeof(Tsumino),
                3 => typeof(Hentai2Read),
                4 => typeof(HentaiNexus),
                _ => typeof(Nhentai),
            };
        }
    }
}
