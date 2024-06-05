using internship.Context.Config;
using internship.Models;
using Microsoft.EntityFrameworkCore;

namespace internship.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //public override int SaveChanges()
        //{
        //    UpdateTimestamps();
        //    return base.SaveChanges();
        //}

        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    UpdateTimestamps();
        //    return base.SaveChangesAsync(cancellationToken);
        //}

        //private void UpdateTimestamps()
        //{
        //    var entries = ChangeTracker
        //        .Entries()
        //        .Where(e => e.Entity is User && (e.State == EntityState.Added || e.State == EntityState.Modified));

        //    foreach (var entry in entries)
        //    {
        //        ((User)entry.Entity).UpdatedTime = DateTime.UtcNow;

        //        if (entry.State == EntityState.Added)
        //        {
        //            ((User)entry.Entity).RegisteredTime = DateTime.UtcNow;
        //        }
        //    }
        //}
        public DbSet<User> Users { set; get; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
   
}
