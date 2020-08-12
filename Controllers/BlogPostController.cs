using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubiconTask.Database;
using RubiconTask.Database.Models;
using RubiconTask.Request;
using RubiconTask.Response.BlogPost;
using RubiconTask.Services;

namespace RubiconTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        BlogDBContext BlogDBContext;
        BlogPostServiceManagement BlogPostServiceManagement;
        public BlogPostController(BlogDBContext blogDBContext)
        {
            BlogDBContext = blogDBContext;
            BlogPostServiceManagement = new BlogPostServiceManagement(blogDBContext);
        }



        [HttpGet]
        public IActionResult Get()
        {
            return Ok(BlogPostServiceManagement.GetAllBlogPosts());
        }

        [HttpGet]
        public IActionResult GetByTag([FromQuery] string tagName)
        {
            return Ok(BlogPostServiceManagement.GetBlogPostsByTagName(tagName));
        }
        [HttpGet("{slug}")]
        public IActionResult GetBySlug(string slug)
        {
            try
            {
                return Ok(BlogPostServiceManagement.GetBlogPostBySlug(slug));
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateBlogPost createBlogPost)
        {

            try
            {
                return Ok(BlogPostServiceManagement.CreateBlogPost(createBlogPost));
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (DuplicateNameException)
            {
                return BadRequest();
            }
        }

        [HttpPut("{slug}")]
        public IActionResult Put([FromBody] UpdateBlogPost updateBlogPost, string slug)
        {
            BlogPostServiceManagement.UpdateBlogPost(updateBlogPost, slug);
            return NoContent();
        }

        [HttpDelete("{slug}")]
        public IActionResult Delete(string slug)
        {


            BlogPostServiceManagement.DeleteBlogPost(slug);
            return NoContent();

        }

    }
}
