

using DispatcherApp.Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = DispatcherApp.Common.Entities.File;

namespace DispatcherApp.DAL.Data
{
    public class AppDbContext: IdentityDbContext
    {
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentUser> AssignmentUsers { get; set; }
        public DbSet<Tutorial> Tutorials { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<DispatcherSession> DispatcherSessions { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<File>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.FileName).IsRequired().HasMaxLength(255);
                entity.Property(f => f.StoragePath).IsRequired();
                entity.HasOne<IdentityUser>(f => f.UploadedByUser)
                    .WithMany()
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Tutorial>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Description).HasMaxLength(1000);
                entity.Property(t => t.Url).HasMaxLength(500);
                entity.HasIndex(t => t.CreatedAt);
                entity.HasOne(t => t.Picture)
                    .WithMany()
                    .HasForeignKey(t => t.PictureId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasMany(t => t.Files)
                    .WithMany(f => f.Tutorials);
            });

            var s = modelBuilder.Entity<DispatcherSession>();
            s.HasKey(x => x.Id);
            s.Property(x => x.RowVersion).IsRowVersion();
            s.Property(x => x.Version).IsConcurrencyToken();
            s.HasOne(x => x.Assignment)
             .WithMany()
             .HasForeignKey(x => x.AssignmentId)
             .OnDelete(DeleteBehavior.Restrict);

            var p = modelBuilder.Entity<SessionParticipant>();
            p.HasKey(x => new { x.SessionId, x.UserId });
            p.HasOne(x => x.User)
             .WithMany()
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            p.HasOne(x => x.Session)
             .WithMany(x => x.Participants)
             .HasForeignKey(x => x.SessionId);

            modelBuilder.Entity<AssignmentUser>(entity =>
            {
                entity.HasKey(au => new { au.AssignmentId, au.UserId });

                entity.HasOne(au => au.Assignment)
                    .WithMany(a => a.AssignmentUsers)
                    .HasForeignKey(au => au.AssignmentId);
                entity.Property(au => au.UserId)
                    .HasMaxLength(450);
            }
            );                
        }
    }
}
