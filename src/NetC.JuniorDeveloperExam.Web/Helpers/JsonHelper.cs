using NetC.JuniorDeveloperExam.Web.Models;
using Newtonsoft.Json;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.IO;
using System;

namespace NetC.JuniorDeveloperExam.Web.Helper
{
    public class JsonHelper : IJsonHelper
    {
        private readonly string filePath = "~/App_Data/Blog-Posts.json";
        private readonly ObjectCache cache = MemoryCache.Default;
        private readonly CacheItemPolicy cachePolicy = new CacheItemPolicy();

        public List<BlogPost> ReadData()
        {
            //check to see if we have the contents in the cache
            if (!(cache["blogPosts"] is string blogPostsCache))
            {
                //Read the file contents and add to the cache  
                blogPostsCache = File.ReadAllText(HttpContext.Current.Server.MapPath(filePath));
                cachePolicy.SlidingExpiration = TimeSpan.FromSeconds(3);
                cache.Set("blogPosts", blogPostsCache, cachePolicy);
            }

            //deserialise into our dto and return as a list of blog posts
            var blogPostsDto = JsonConvert.DeserializeObject<BlogPostsDto>(blogPostsCache);
            return new List<BlogPost>(blogPostsDto.BlogPosts);
        }

        public void UpdateData(List<BlogPost> blogPosts)
        {
            var blogPostsDto = new BlogPostsDto(blogPosts);
            var serializedDto = JsonConvert.SerializeObject(blogPostsDto);
            File.WriteAllText(HttpContext.Current.Server.MapPath(filePath), serializedDto);

            cache.Set("blogPosts", serializedDto, cachePolicy);
        }

        public List<BlogPost> GetBlogPosts()
        {
            return ReadData();
        }

        public BlogPost GetBlogPostById(int id)
        {
            var blogPosts = ReadData();
            var blogPost = blogPosts.FirstOrDefault(x => x.Id == id);

            return blogPost;
        }

        public IEnumerable<Comment> GetBlogPostCommentsById(int id)
        {
            return GetBlogPostById(id).Comments;
        }

        public BlogPost AddCommentToBlogPost(Comment comment)
        {
            var blogPosts = ReadData();
            var comments = blogPosts.FirstOrDefault(x => x.Id == comment.OriginalBlogPostId).Comments;

            comment.Date = DateTime.Now;

            if (comments != null)
            {
                comment.Id = comments.Count() + 1;
                blogPosts.FirstOrDefault(x => x.Id == comment.OriginalBlogPostId).Comments.Add(comment);
            }
            else
            {
                comment.Id = 1;
                blogPosts.FirstOrDefault(x => x.Id == comment.OriginalBlogPostId).Comments = new List<Comment> { comment };
            }

            UpdateData(blogPosts);

            return blogPosts.FirstOrDefault(x => x.Id == comment.OriginalBlogPostId);
        }

        public BlogPost AddReplyToComment(Comment comment, Comment reply)
        {
            var blogPosts = ReadData();
            List<Comment> replyList = new List<Comment>();
        
            if (comment.Replies != null)
            {
                replyList.AddRange(comment.Replies);
            }
        
            replyList.Add(reply);
        
            blogPosts.FirstOrDefault(x => x.Id == reply.OriginalBlogPostId).Comments
                .FirstOrDefault(x => x.Id == reply.OriginalCommentId)
                .Replies = replyList;
        
            UpdateData(blogPosts);
        
            return blogPosts.FirstOrDefault(x => x.Id == reply.OriginalBlogPostId);
        }
    }
}