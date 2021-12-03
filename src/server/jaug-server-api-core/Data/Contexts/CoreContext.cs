using jaug_server_api_core.Data.Entities;
using jaug_server_api_core.Data.Entities.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace jaug_server_api_core.Data.Contexts
{
    public class CoreContext : DbContext
    {
        public CoreContext(DbContextOptions<CoreContext> options) : base(options)
        {

        }

        public DbSet<Tool> Tools { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            // !!! tmp
            // !!! add service for datetime/user
            var dt = DateTime.UtcNow;
            var userId = "UNKNOWN";

            // ??? validate date/time for lastmodified being updated where created is not
            // ??? isn't createdon/lastmodified dealt with - just need to address user in cases below since relying on DB for date/time stuff ??? 


            foreach (var entry in ChangeTracker.Entries<IEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = dt;
                        entry.Entity.CreatedBy = userId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = dt;
                        entry.Entity.LastModifiedBy = userId;
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        // being explicit about relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Tool>()
                .HasMany(t => t.Commands)       // Tool relates to many Commands
                .WithOne(c => c.Tool!)          // Each Command relates to one Tool
                .HasForeignKey(t => t.ToolId);  // Each Tool can be used as a ForeignKey

            modelBuilder
                .Entity<Command>()
                .HasOne(c => c.Tool)            // Command relates to one Tool
                .WithMany(t => t.Commands)      // Each Tool may relate to many Commands
                .HasForeignKey(c => c.ToolId);  // Each Command can be used as a ForeignKey

            var t = modelBuilder.Entity<Tool>();
       
            t.Property(p => p.CreatedOn).HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd(); // !!! GETUTCDATE() mixing concerns (DB Type) - yuck !!!
            t.Property(p => p.LastModifiedOn).HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnUpdate();
            t.Property(p => p.CreatedBy).HasDefaultValue("SEED").ValueGeneratedOnAdd(); // !!! SEED - tmp
            t.Property(p => p.LastModifiedBy).HasDefaultValue("SEED").ValueGeneratedOnUpdate(); // !!! SEED - tmp

            var c = modelBuilder.Entity<Command>();
            c.Property(p => p.CreatedOn).HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd();
            c.Property(p => p.LastModifiedOn).HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnUpdate();
            c.Property(p => p.CreatedBy).HasDefaultValue("SEED").ValueGeneratedOnAdd(); // !!! SEED - tmp
            c.Property(p => p.LastModifiedBy).HasDefaultValue("SEED").ValueGeneratedOnUpdate(); // !!! SEED - tmp

            modelBuilder.Entity<Tool>()
                .HasData(
                new { Id = 1, Name = "dotnet", Description = "" },
                new { Id = 2, Name = "docker", Description = "" },
                new { Id = 3, Name = "git", Description = "" });

            modelBuilder.Entity<Command>().HasData(
                new { Id = 1, Description = "Enable secret storage for project", Syntax = "dotnet user-secrets init", ToolId = 1 },
                new { Id = 2, Description = "Set secret for project", Syntax = "dotnet user-secrets set \"<key>\" \"<value>\"", ToolId = 1 },
                new { Id = 3, Description = "Generate default dotnet gitignore file for project", Syntax = "dotnet new gitignore", ToolId = 1 },
                new { Id = 4, Description = "Create an empty Git repository or reinitialize an existing one", Syntax = "git init", ToolId = 3 });
        }
    }
}
