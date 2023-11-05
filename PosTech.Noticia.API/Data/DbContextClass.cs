using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PosTech.Noticia.API.Models;

namespace PosTech.Noticia.API.Data
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration _configuration;

        public DbContextClass()
        { }

        public DbContextClass(DbContextOptions<DbContextClass> options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql(_configuration.GetConnectionString("WebApiDatabase"));
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");
        
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContextClass).Assembly);
        
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.Cascade;
        
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Usuarios> Usuario { get; set; }
        public virtual DbSet<Models.Noticias> Noticias { get; set; }
    }
}
