using System.Collections.Generic;

namespace NetC.JuniorDeveloperExam.Web.Models
{
    public class BlogPostsDto
    {
        public BlogPostsDto(List<BlogPost> blogPosts)
        {
            BlogPosts = blogPosts.ToArray();        
        }        
        public BlogPost[] BlogPosts { get; set; }
    }
}