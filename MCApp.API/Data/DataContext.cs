using MCApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MCApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Mutation> Mutations { get; set; }
        public DbSet<Interest> Interests { get; set; }
        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<User>()
                .HasAlternateKey(u => u.Username);
                
            builder.Entity<User>()
                .HasIndex(u => u.Username);
                
            builder.Entity<Mutation>()
                .HasIndex(m => new {m.UserId, m.AccountId});
                
            builder.Entity<Mutation>()
                .HasIndex(m => m.Created);
                
            builder.Entity<Mutation>()
                .HasAlternateKey(m => m.PrevId);
        }
    }
    
}