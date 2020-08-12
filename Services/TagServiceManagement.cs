using RubiconTask.Database;
using RubiconTask.Database.Models;
using RubiconTask.Request.Tag;
using RubiconTask.Response.Tag;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconTask.Services
{
    public class TagServiceManagement
    {
        BlogDBContext BlogDBContext;
        public TagServiceManagement(BlogDBContext blogDBContext) {
            BlogDBContext = blogDBContext;
        }

        public List<TagDto> GetTag() {
            List<Tag> tags = BlogDBContext.Tags.ToList();
            List<TagDto> tagDtos = new List<TagDto>();

            for (int i = 0; i < tags.Count; i++) {
                TagDto tagDto = new TagDto();
                tagDto.Name = tags[i].Name;
                tagDtos.Add(tagDto);
            }
            return tagDtos;
        }

        public TagDto CreateTag(CreateTag createTag) {
            ValidateTag(createTag.Name);
            Tag tag = new Tag();
            tag.Name = createTag.Name;
            BlogDBContext.Tags.Add(tag);
            BlogDBContext.SaveChanges();

            TagDto tagDto = new TagDto();
            tagDto.Name = tag.Name;
            return tagDto;
        }

        private void ValidateTag(String name)
        {
            Tag tag = BlogDBContext.Tags.Where(t => t.Name == name).FirstOrDefault();
            if (tag != null) {
                throw new DuplicateNameException();
            }
        }
    }
}
