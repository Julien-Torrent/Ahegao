using Ahegao.Models;
using Ahegao.SitesParsers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ahegao.Data
{
    public class SiteContext : DbContext
    {
        public DbSet<Site> Sites { get; set; }

        public SiteContext(DbContextOptions<SiteContext> options) : base(options)
        { 
        }

        public async Task<List<Site>> GetAllAsync()
        {
            return await Sites.ToListAsync();
        }

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
