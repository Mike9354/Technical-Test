using NetC.JuniorDeveloperExam.Web.Helper;
using NetC.JuniorDeveloperExam.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NetC.JuniorDeveloperExam.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJsonHelper _jsonHelper;
        private readonly List<BlogPost> _blogPosts;

        public HomeController(IJsonHelper jsonHelper)
        {
            _jsonHelper = jsonHelper;
            _blogPosts = _jsonHelper.GetBlogPosts();
        }

        public ActionResult Index()
        {
            return View(_blogPosts);
        }

        public ActionResult Blog(int id)
        {
            return View(_blogPosts.FirstOrDefault(x => x.Id == id));
        }

        [Route("comments")]
        public ActionResult Comments(int id)
        {
            return Json(_jsonHelper.GetBlogPostById(id), JsonRequestBehavior.AllowGet);
        }

        [Route("addComment")]
        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
            Comment parentComment = null;
            
            var blogPost = _jsonHelper.GetBlogPostById(comment.OriginalBlogPostId);

            comment.Date = DateTime.Now;
            if (comment.OriginalCommentId != null)
            {
                int commentCount = 0;
                parentComment = blogPost.Comments.FirstOrDefault(x => x.Id == comment.OriginalCommentId);
                if(parentComment.Replies != null && parentComment.Replies.Any())
                {
                    commentCount = parentComment.Replies.Count();
                }
            
                comment.Id = commentCount + 1;            
            
                _jsonHelper.AddReplyToComment(parentComment, comment);            
            }
            else
            {
                int commentCount = 0;

                if(blogPost.Comments != null)
                {
                    commentCount = blogPost.Comments.Count();
                }

                comment.Id = commentCount + 1;
                _jsonHelper.AddCommentToBlogPost(comment);
            }
            
            return Content("Success :)");
        }
    }
}