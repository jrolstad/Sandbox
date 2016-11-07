using System.Collections.Generic;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApplication.models
{
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class Post
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
}