using Announcements.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Announcements.Infrastructure.Persistence
{
    /// <summary>
    /// DB context for application resposible for interacting with announcement table.
    /// </summary>
    public class AnnouncemetDbContext : DbContext
    {
        public AnnouncemetDbContext(DbContextOptions<AnnouncemetDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Collection of announcements in DB.
        /// </summary>
        public DbSet<Announcement> Announcements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.DateAdded).IsRequired();
                entity.HasIndex(e => e.DateAdded);
            });
        }
    }
}