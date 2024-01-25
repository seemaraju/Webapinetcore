using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute.Data.Models;
using Microsoft.Extensions.Configuration;

namespace Attribute.Data.Entity
{
    public class AppDbContext : DbContext
    {
        //public AppDbContext(DbContextOptions<AppDbContext> options) 
        //    : base(options)
        //{

        //}
        //public DbSet<BulkAttributeUpdateImport> BulkAttributeUpdateImport { get; set; }

        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("POAttributeUpdateConnection"));
        }

        public DbSet<BulkAttributeUpdateImport> BulkAttributeUpdateImport { get; set; }
    }
}
