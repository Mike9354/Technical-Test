using System;
using System.Collections.Generic;

namespace NetC.JuniorDeveloperExam.Web.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int OriginalBlogPostId { get; set; }
        public int? OriginalCommentId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public string EmailAddress { get; set; }
        public string Message { get; set; }
        public IEnumerable<Comment> Replies { get; set; }
    }
}