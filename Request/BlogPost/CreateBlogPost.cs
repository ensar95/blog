using RubiconTask.Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconTask.Request
{
    public class CreateBlogPost
    {
        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage ="Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage ="Body is required")]
        public string Body { get; set; }
        public List<string> TagList { get; set; }
    }
}
