using BloggingSiteCMS.DAL.Domain;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BloggingSiteCMS.DAL
{
    public class BloggingSiteCMSContext : IdentityDbContext<AppUser>
    {
        public BloggingSiteCMSContext(DbContextOptions<BloggingSiteCMSContext> options) :
            base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Posts);

            builder.Entity<Post>()
                .HasMany(p => p.Tags)
                .WithMany(c => c.Posts);
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
    }
}