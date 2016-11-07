using System;
using ConsoleApplication.models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using(var context = new BloggingContext())
            {
                var blog = context.Blogs
                 .FirstOrDefault(b=>b.Name == "MyBlog");
                if(blog == null)
                {
                    blog = new Blog{Name="MyBlog",BlogId = 1};
                    context.Blogs.Add(blog);
                }

                var now = DateTime.Now.Ticks;
                var nextPostId = context.Posts.Select(p=>p.PostId).Max() + 1;
                var post = new Post{Title=$"Blog of the Moment - {now}",Blog = blog,PostId = nextPostId};
                context.Posts.Add(post);

                context.SaveChanges();

                foreach(var blogToRead in context.Blogs.Include(b=>b.Posts))
                {
                    foreach(var postToRead in blogToRead.Posts)
                    {
                        var blogName = blogToRead.Name;
                        var postName = postToRead.Title;
                        Console.WriteLine($"Blog:{blogName},Post:{postName}");
                    }
                }
            }
        }
    }
}
