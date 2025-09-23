

using DispatcherApp.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = DispatcherApp.Models.Entities.File;

namespace DispatcherApp.DAL.Data
{
    public class AppDbContext: IdentityDbContext
    {
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Tutorial> Tutorials { get; set; }
        public DbSet<File> Files { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Tutorial>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Description).HasMaxLength(1000);
                entity.Property(t => t.Url).HasMaxLength(500);
                entity.HasIndex(t => t.CreatedAt);
            });
            modelBuilder.Entity<File>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.FileName).IsRequired().HasMaxLength(255);
                entity.Property(f => f.StoragePath).IsRequired();
            });

            modelBuilder.Entity<Tutorial>()
            .HasMany(t => t.Files)
            .WithMany(f => f.Tutorials);
        }
    }
}
