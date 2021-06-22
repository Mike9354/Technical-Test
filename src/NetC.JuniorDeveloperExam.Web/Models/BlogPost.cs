using System;
using System.Collections.Generic;

namespace NetC.JuniorDeveloperExam.Web.Models
{

    public class BlogPost
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string HtmlContent { get; set; }
        public List<Comment> Comments { get; set; }
    }
}