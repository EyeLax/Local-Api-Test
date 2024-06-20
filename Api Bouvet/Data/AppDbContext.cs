using Api_Bouvet.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_Bouvet.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship between Project and Epic
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Epics)
                .WithOne(e => e.Project)
                .HasForeignKey(e => e.ProjectId);

            // Configure the one-to-many relationship between Epic and Task
            modelBuilder.Entity<Epic>()
                .HasMany(e => e.Tasks)
                .WithOne(t => t.Epic)
                .HasForeignKey(t => t.EpicId);
        }
    }
}
