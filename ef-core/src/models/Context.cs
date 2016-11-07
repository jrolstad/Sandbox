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
        optionsBuilder.UseSqlite("Filename=./blog.db");
    }
}
}