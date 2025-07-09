using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TMS.Domain.Entities;
using TMS.Infrastructure.Data.Configuration;

namespace TMS.Infrastructure.Data
{
    public sealed class TmsDbContext : DbContext
    {
        public TmsDbContext(DbContextOptions<TmsDbContext> options) : base(options) 
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Domain.Entities.Task> Tasks => Set<Domain.Entities.Task>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
        }
    }
}
