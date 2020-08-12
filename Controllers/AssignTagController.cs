using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubiconTask.Database;
using RubiconTask.Database.Models;
using RubiconTask.Request.BlogPostTag;
using RubiconTask.Services;

namespace RubiconTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignTagController : ControllerBase
    {
        BlogDBContext BlogDBContext;
        BlogPostServiceManagement BlogPostServiceManagement;
        public AssignTagController(BlogDBContext blogDBContext)
        {
            BlogPostServiceManagement = new BlogPostServiceManagement(blogDBContext);
        }
        [HttpPost]
        public IActionResult AssignBlogPost(CreateBlogPostTag createBlogPostTag) {
            try
            {
                return Ok(BlogPostServiceManagement.AssignTagToBlogPost(createBlogPostTag));
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }

        }
    }
}
