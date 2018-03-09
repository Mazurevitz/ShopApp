using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShopApp.Models
{
    public class ShopAppContext : DbContext
    {
        public ShopAppContext (DbContextOptions<ShopAppContext> options)
            : base(options)
        {
        }

        public DbSet<ShopApp.Models.Notebook> Notebook { get; set; }
    }
}
