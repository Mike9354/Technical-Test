using NetC.JuniorDeveloperExam.Web.Models;
using System.Collections.Generic;

namespace NetC.JuniorDeveloperExam.Web.Helper
{
    public interface IJsonHelper
    {
        List<BlogPost> ReadData();
        void UpdateData(List<BlogPost> blogPosts);
        List<BlogPost> GetBlogPosts();
        BlogPost GetBlogPostById(int id);
        IEnumerable<Comment> GetBlogPostCommentsById(int id);
        BlogPost AddCommentToBlogPost(Comment comment);
        BlogPost AddReplyToComment(Comment comment, Comment reply);
    }
}