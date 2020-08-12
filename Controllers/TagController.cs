using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubiconTask.Database;
using RubiconTask.Request.Tag;
using RubiconTask.Response.Tag;
using RubiconTask.Services;

namespace RubiconTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        BlogDBContext BlogDBContext;
        TagServiceManagement TagServiceManagement;
        public TagController(BlogDBContext blogDBContext)
        {
            BlogDBContext = blogDBContext;
            TagServiceManagement = new TagServiceManagement(blogDBContext);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(TagServiceManagement.GetTag());
        }

        [HttpPost]
        public IActionResult Post(CreateTag createTag)
        {
            try { 
                return Ok(TagServiceManagement.CreateTag(createTag)); 
            } catch (DuplicateNameException e) {
                return BadRequest(); 
            }
        }
    }
}
