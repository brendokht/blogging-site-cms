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
    }
}