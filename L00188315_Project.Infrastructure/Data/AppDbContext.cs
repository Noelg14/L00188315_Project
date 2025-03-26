using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L00188315_Project.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace L00188315_Project.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Consent> Consents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>().HasKey(a => a.AccountId);
            modelBuilder.Entity<Transaction>().HasKey(t => t.TransctionId);
            modelBuilder.Entity<Balance>().HasKey(b => b.AccountId);
            modelBuilder.Entity<Consent>().HasKey(c => c.ConsentId);

            modelBuilder
                .Entity<Balance>()
                .HasOne(b => b.Account)
                .WithOne(a => a.Balance)
                .HasForeignKey<Balance>(b => b.AccountId);
            modelBuilder
                .Entity<Transaction>()
                .HasOne(b => b.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId);

            modelBuilder.Entity<Consent>();
            modelBuilder.Entity<Consent>().HasMany(c => c.Account);

            modelBuilder.Entity<Account>().Property(a => a.Updated).ValueGeneratedOnUpdate();
            modelBuilder.Entity<Account>().Property(a => a.Created).ValueGeneratedOnAdd();

            //modelBuilder.Entity<Consent>().Property(a => a.Updated).ValueGeneratedOnUpdate();
            //modelBuilder.Entity<Consent>().Property(a => a.Created).HasDefaultValue(DateTimeOffset.Now);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<Consent>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.Created = DateTime.UtcNow;
                    entry.Entity.Updated = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.Updated = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default
        )
        {
            foreach (var entry in ChangeTracker.Entries<Consent>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.Created = DateTime.UtcNow;
                    entry.Entity.Updated = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.Updated = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
