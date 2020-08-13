using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using RubiconTask.Database;
using RubiconTask.Database.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Database
{
    public class Data
    {
        public static void Migrate(BlogDBContext blogDBContext) {
            if (!blogDBContext.Database.GetService<IRelationalDatabaseCreator>().Exists())
            {
                Console.WriteLine("Performing database migration!");
            }
            blogDBContext.Database.Migrate();
        }

        public static void Seed (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>().HasData(
                    new BlogPost
                    {
                        Id = 1,
                        Slug = "migrating-data",
                        Title = "Migrating data",
                        Description = "We're migrating data so everyone who starts the app has it!",
                        Body = "This is migrated data, feel free to delete it",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new BlogPost
                    {
                        Id = 2,
                        Slug = "another-migrated-data",
                        Title = "Another Migrated Data",
                        Description = "We're migrating some more data so it looks a bit richer",
                        Body = "This is another migrated blogpost so we have more now",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
                );
            modelBuilder.Entity<Tag>().HasData(
                    new Tag { Id = 1, Name = "Ios" },
                    new Tag { Id = 2, Name = "AngularJS" },
                    new Tag { Id = 3, Name = "Migration" }
                );

            modelBuilder.Entity<BlogPostTags>().HasData(
                new BlogPostTags { BlogPostId = 1, TagId = 1 },
                new BlogPostTags { BlogPostId = 1, TagId = 2 },
                new BlogPostTags { BlogPostId = 2, TagId = 1 },
                new BlogPostTags { BlogPostId = 2, TagId = 3 }
            );
        }
    }
}
