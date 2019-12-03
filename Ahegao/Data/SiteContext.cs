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
            switch(id)
            {
                case 1: return typeof(Nhentai);
                case 2: return typeof(Tsumino);
                case 3: return typeof(Hentai2Read);
                default: return typeof(Nhentai);
            }
        }
    }
}
