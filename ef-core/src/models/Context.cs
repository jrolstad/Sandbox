using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApplication.models
{
    public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = @"Server=(localdb)\mssqllocaldb;Database=my-efcore-db;Trusted_Connection=True;";
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .Property(b => b.BlogId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Post>()
                .Property(b => b.PostId)
                .ValueGeneratedOnAdd();
                
        }
}
}