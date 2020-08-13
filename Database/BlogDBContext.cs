using Blog.Database;
using Microsoft.EntityFrameworkCore;
using RubiconTask.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconTask.Database
{
    public class BlogDBContext : DbContext
    {
        public BlogDBContext(DbContextOptions options) : base(options) {}

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostTags> BlogPostTags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPostTags>().HasKey(m => new { m.BlogPostId, m.TagId });
            modelBuilder.Entity<BlogPostTags>()
                .HasOne(m => m.BlogPost)
                .WithMany(m => m.BlogPostTags)
                .HasForeignKey(m => m.BlogPostId);

            modelBuilder.Entity<BlogPostTags>()
                .HasOne(m => m.Tag)
                .WithMany(m => m.BlogPostTags)
                .HasForeignKey(m => m.TagId);

            Data.Seed(modelBuilder);
        }
    }
}
