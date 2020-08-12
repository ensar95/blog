using Microsoft.EntityFrameworkCore;
using RubiconTask.Database;
using RubiconTask.Database.Models;
using RubiconTask.Request;
using RubiconTask.Request.BlogPostTag;
using RubiconTask.Response.BlogPost;
using RubiconTask.Response.BlogPostTag;
using Slugify;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace RubiconTask.Services
{
    public class BlogPostServiceManagement
    {
        BlogDBContext BlogDBContext;

        public BlogPostServiceManagement()
        {
        }

        public BlogPostServiceManagement(BlogDBContext blogDBContext)
        {
            BlogDBContext = blogDBContext;
        }

      

        public BlogPostList GetBlogPosts(string tagName)
        {
            BlogPostList blogPostList = new BlogPostList();

            if(tagName == null)
            {
                blogPostList.BlogPostDtos = BlogDBContext.BlogPosts
                    .Select(x => MapFromDb(x))
                    .ToList();
            }
            else
            {
                blogPostList.BlogPostDtos = BlogDBContext.BlogPostTags
                .Where(x => x.Tag.Name == tagName)
                .Select(x => MapFromDb(x.BlogPost))
                .ToList();
            }

            return blogPostList;
        }
       
        public BlogPostDto GetBlogPostBySlug(string slug)
        {
            BlogPost blogPost = BlogDBContext.BlogPosts.Where(m => m.Slug == slug).FirstOrDefault();
            if (blogPost == null)
            {
                throw new ArgumentException();
            }

            BlogPostDto blogPostDto = MapFromDb(blogPost);
            
            try
            {
                if (blogPost.BlogPostTags.ToList() != null || blogPost.BlogPostTags.Count != 0)
                {
                    List<BlogPostTags> blogPostTags = blogPost.BlogPostTags.ToList();
                    foreach (BlogPostTags blog in blogPostTags)
                    {
                        blogPostDto.TagList.Add(blog.Tag.Name);
                    }
                }
                return blogPostDto;
            }
            catch (ArgumentNullException e)
            {
                return blogPostDto;
            }
        }
        public BlogPostDto CreateBlogPost(CreateBlogPost createBlogPost)
        {
            if (createBlogPost.Title == null)
            {
                throw new ArgumentNullException();
            }

            BlogPost blogPost = new BlogPost();

            SlugHelper slugHelper = new SlugHelper();
            string Slug = slugHelper.GenerateSlug(createBlogPost.Title);
            ValidateBlogPost(Slug);

            blogPost.Title = createBlogPost.Title;
            blogPost.Slug = Slug;
            blogPost.Description = createBlogPost.Description;
            blogPost.Body = createBlogPost.Body;

            blogPost.CreatedAt = DateTime.Now;
            blogPost.UpdatedAt = DateTime.Now;
            BlogDBContext.BlogPosts.Add(blogPost);
            BlogDBContext.SaveChanges();

            BlogPostDto blogPostDto = new BlogPostDto
            {
                Slug = blogPost.Slug,
                Title = blogPost.Title,
                Description = blogPost.Description,
                Body = blogPost.Body
            };

            blogPostDto.CreatedAt = blogPost.CreatedAt;
            blogPostDto.UpdatedAt = blogPost.UpdatedAt;
            return blogPostDto;
        }
        public BlogPostTagDto AssignTagToBlogPost(CreateBlogPostTag createBlogPostTag)
        {
            BlogPost blogPost = BlogDBContext.BlogPosts.Where(s => s.Id == createBlogPostTag.BlogPostId).FirstOrDefault();
            if (blogPost == null)
            {
                throw new ArgumentNullException();
            }
            Tag tag = BlogDBContext.Tags.Where(t => t.Id == createBlogPostTag.TagId).FirstOrDefault();
            if (tag == null)
            {
                throw new ArgumentNullException();
            }

            BlogPostTags blogPostTags = new BlogPostTags();
            blogPostTags.BlogPostId = createBlogPostTag.BlogPostId;
            blogPostTags.TagId = createBlogPostTag.TagId;
            blogPostTags.BlogPost = blogPost;
            blogPostTags.Tag = tag;
            BlogPostTagDto blogPostTagDto = new BlogPostTagDto();
            blogPostTagDto.BlogPostId = blogPost.Id;
            blogPostTagDto.TagId = tag.Id;
            BlogDBContext.BlogPostTags.Add(blogPostTags);
            BlogDBContext.SaveChanges();
            return blogPostTagDto;

        }

        public void UpdateBlogPost(UpdateBlogPost updateBlogPost, string slug)
        {
            BlogPost blogPost = BlogDBContext.BlogPosts.Where(m => m.Slug == slug).FirstOrDefault();

            if (updateBlogPost.Title != null)
            {
                SlugHelper slugHelper = new SlugHelper();
                string Slug = slugHelper.GenerateSlug(updateBlogPost.Title);

                blogPost.Slug = Slug;
            }
            if (updateBlogPost.Description != null)
            {
                blogPost.Description = updateBlogPost.Description;
            }
            if (updateBlogPost.Body != null)
            {
                blogPost.Body = updateBlogPost.Body;
            }

            blogPost.UpdatedAt = DateTime.Now;
            BlogDBContext.BlogPosts.Update(blogPost);
            BlogDBContext.SaveChanges();
        }

        public void DeleteBlogPost(string slug)
        {
            BlogPost blogPost = BlogDBContext.BlogPosts.Where(m => m.Slug == slug).FirstOrDefault();
            BlogDBContext.BlogPosts.Remove(blogPost);
            BlogDBContext.SaveChanges();
        }

        private static BlogPostDto MapFromDb(BlogPost blogPost)
        {
            BlogPostDto blogPostDto = new BlogPostDto();
            blogPostDto.Slug = blogPost.Slug;
            blogPostDto.Title = blogPost.Title;
            blogPostDto.Description = blogPost.Description;
            blogPostDto.Body = blogPost.Body;
            blogPostDto.CreatedAt = blogPost.CreatedAt;
            blogPostDto.UpdatedAt = blogPost.UpdatedAt;

            return blogPostDto;
        }
        private void ValidateBlogPost(string slug)
        {
            BlogPost blogPost = BlogDBContext.BlogPosts.Where(b => b.Slug == slug).FirstOrDefault();
            if (blogPost != null)
            {
                throw new DuplicateNameException();
            }
        }

    }
}