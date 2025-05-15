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
            modelBuilder.Entity<Account>().HasKey(a => a.Id);
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
            modelBuilder.Entity<Balance>().HasKey(b => b.BalanceId);
            modelBuilder.Entity<Consent>().HasKey(c => c.ConsentId);

            modelBuilder
                .Entity<Account>()
                .HasMany(t => t.Transactions)
                .WithOne(a => a.Account)
                .HasForeignKey(t => t.RootAccountId)
                .HasPrincipalKey(a => a.Id)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder
                .Entity<Account>()
                .HasOne(b => b.Balance)
                .WithOne(a => a.Account)
                .HasForeignKey<Balance>(b => b.RootAccountId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder
                .Entity<Balance>()
                .HasOne(a => a.Account)
                .WithOne(b => b.Balance)
                .OnDelete(DeleteBehavior.Cascade);

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
