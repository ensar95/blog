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

        private void ValidateBlogPost(String slug)
        {
            BlogPost blogPost = BlogDBContext.BlogPosts.Where(b => b.Slug == slug).FirstOrDefault();
            if (blogPost != null)
            {
                throw new DuplicateNameException();
            }
        }

        public BlogPostDto CreateBlogPost(CreateBlogPost createBlogPost)
        {
            if (createBlogPost.Title == null)
            {
                throw new ArgumentNullException();
            }

            BlogPost blogPost = new BlogPost
            {
                Title = createBlogPost.Title
            };

            SlugHelper slugHelper = new SlugHelper();
            string Slug = slugHelper.GenerateSlug(createBlogPost.Title);

            blogPost.Slug = Slug;
            blogPost.Description = createBlogPost.Description;
            blogPost.Body = createBlogPost.Body;
            ValidateBlogPost(Slug);
            List<string> TagList = new List<string>();


            if (createBlogPost.TagList != null && createBlogPost.TagList.Count != 0)
            {
                for (int i = 0; i < createBlogPost.TagList.Count; i++)
                {
                    BlogPostTags blogPostTags = new BlogPostTags();
                    TagList.Add(createBlogPost.TagList[i]);
                    Tag Tag = BlogDBContext.Tags.Where(m => m.Name == TagList[i]).FirstOrDefault();
                    blogPostTags.TagId = Tag.Id;
                    blogPostTags.Tag = Tag;
                    blogPost.BlogPostTags.Add(blogPostTags);
                }
            }


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

            for (int i = 0; i < TagList.Count; i++)
            {
                blogPostDto.TagList.Add(TagList[i]);
            }

            blogPostDto.CreatedAt = blogPost.CreatedAt;
            blogPostDto.UpdatedAt = blogPost.UpdatedAt;
            return blogPostDto;
        }

        public List<BlogPostDto> GetBlogPostsByTagName(string tagName)
        {
            List<BlogPostTags> blogPostTags = new List<BlogPostTags>();
            List<BlogPost> blogPosts = BlogDBContext.BlogPosts.ToList();
            List<BlogPostDto> blogPostDtos = new List<BlogPostDto>();
            for (int i = 0; i < blogPosts.Count(); i++)
            {
                int v = blogPosts[i].BlogPostTags.Count();
                for (int j = 0; j < v; j++)
                {
                    BlogPostTags blogPostTag = blogPosts[i].BlogPostTags[j];
                    blogPostTags.Add(blogPostTag);
                    if (blogPostTag.Tag.Name == tagName)
                    {
                        BlogPostDto blogPostDto = new BlogPostDto();
                        blogPostDto.Slug = blogPosts[i].Slug;
                        blogPostDto.Title = blogPosts[i].Title;
                        blogPostDto.Description = blogPosts[i].Description;
                        blogPostDto.Body = blogPosts[i].Body;
                        blogPostDto.CreatedAt = blogPosts[i].CreatedAt;
                        blogPostDto.UpdatedAt = blogPosts[i].UpdatedAt;
                        blogPostDtos.Add(blogPostDto);
                    }
                }
            }
            return blogPostDtos;

        }

        private List<BlogPostDto> GetBlogPosts()
        {
            List<BlogPost> blogPosts = BlogDBContext.BlogPosts.ToList();
            List<BlogPostDto> blogPostDtos = new List<BlogPostDto>();

            for (int i = 0; i < blogPosts.Count; i++)
            {
                BlogPost blogPost = blogPosts[i];
                BlogPostDto blogPostDto = new BlogPostDto
                {
                    Slug = blogPost.Slug,
                    Title = blogPost.Title,
                    Description = blogPost.Description,
                    Body = blogPost.Body
                };
                try
                {
                    if (blogPost.BlogPostTags.ToList() != null || blogPost.BlogPostTags.ToList().Count != 0)
                    {
                        List<BlogPostTags> blogPostTags = blogPost.BlogPostTags.ToList();
                        foreach (BlogPostTags blog in blogPostTags)
                        {
                            blogPostDto.TagList.Add(blog.Tag.Name);
                        }
                    }
                    blogPostDto.CreatedAt = blogPost.CreatedAt;
                    blogPostDto.UpdatedAt = blogPost.UpdatedAt;
                    blogPostDtos.Add(blogPostDto);

                }
                catch (ArgumentNullException)
                {
                    blogPostDto.CreatedAt = blogPost.CreatedAt;
                    blogPostDto.UpdatedAt = blogPost.UpdatedAt;
                    blogPostDtos.Add(blogPostDto);
                }

            }
            return blogPostDtos;

        }

        public BlogPostList GetAllBlogPosts()
        {
            BlogPostList blogPostList = new BlogPostList();
            blogPostList.BlogPostDtos = new List<BlogPostDto>();
            List<BlogPostDto> postDtos = GetBlogPosts();
            for (int i = 0; i < postDtos.Count(); i++)
            {
                blogPostList.BlogPostDtos.Add(postDtos[i]);
            }
            blogPostList.NumberOfPosts = "Number of posts: " + blogPostList.BlogPostDtos.Count();
            return blogPostList;
        }

        public BlogPostDto GetBlogPostBySlug(string slug)
        {
            BlogPost blogPost = BlogDBContext.BlogPosts.Where(m => m.Slug == slug).FirstOrDefault();
            if (blogPost == null)
            {
                throw new ArgumentException();
            }
            BlogPostDto blogPostDto = new BlogPostDto();
            blogPostDto.Slug = blogPost.Slug;
            blogPostDto.Title = blogPost.Title;
            blogPostDto.Description = blogPost.Description;
            blogPostDto.Body = blogPost.Body;
            blogPostDto.CreatedAt = blogPost.CreatedAt;
            blogPostDto.UpdatedAt = blogPost.UpdatedAt;
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
    }
}