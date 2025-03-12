using L00188315_Project.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Balance> Balances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>().HasKey(a => a.AccountId);
            modelBuilder.Entity<Transaction>().HasKey(t => t.TransctionId);
            modelBuilder.Entity<Balance>().HasKey(b => b.AccountId);
            modelBuilder.Entity<Balance>().HasOne(b => b.Account).WithOne(a => a.Balance).HasForeignKey<Balance>(b => b.AccountId);
            modelBuilder.Entity<Transaction>().HasOne(b => b.Account).WithMany(a => a.Transactions).HasForeignKey(t => t.AccountId);

            modelBuilder.Entity<Account>().Property(a => a.Updated).ValueGeneratedOnUpdate();
            modelBuilder.Entity<Account>().Property(a => a.Created).ValueGeneratedOnAdd();


        }
    }
}
