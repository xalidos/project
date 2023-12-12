using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Mina.Entities;

namespace Mina.DbContexts
{
    public class MinaDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public MinaDbContext(DbContextOptions<MinaDbContext> options) : base(options)
        {
        }

        public DbSet<Building> Buildings { get; set; }
        //public DbSet<POI> Poi { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("postgis");
        }
    }
}
