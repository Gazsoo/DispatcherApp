

using DispatcherApp.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DispatcherApp.DAL.Data
{
    public class AppDbContext: IdentityDbContext
    {
        public DbSet<Assignment> Assignments => Set<Assignment>();
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
