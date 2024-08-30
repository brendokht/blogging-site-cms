using BloggingSiteCMS.DAL.Domain;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BloggingSiteCMS.DAL
{
    public class CMSContext : IdentityDbContext<AppUser, AppRole, string, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        public CMSContext() { }

        public CMSContext(DbContextOptions<CMSContext> options) :
            base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
                .AddEnvironmentVariables()
                .AddUserSecrets<CMSContext>()
                .Build();
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>(p =>
            {
                p.HasMany(p => p.Categories)
                .WithMany(c => c.Posts);

                p.HasMany(p => p.Tags)
                .WithMany(c => c.Posts);
            });

            builder.Entity<Comment>(c =>
            {
                c.HasOne(c => c.Author)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AppUserId)
                .OnDelete(DeleteBehavior.SetNull);

                c.HasOne(c => c.Post)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<AppUser>(u =>
            {
                u.HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

                u.HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

                u.HasMany(e => e.Tokens)
                .WithOne()
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

                u.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });

            builder.Entity<AppRole>(r =>
            {
                r.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

                r.HasMany(e => e.RoleClaims)
                .WithOne(e => e.Role)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();
            });

            builder.Entity<AppUserClaim>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.Claims)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            builder.Entity<AppUserLogin>()
                .HasOne(ul => ul.User)
                .WithMany(u => u.Logins)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            builder.Entity<AppUserToken>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.Tokens)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            foreach (var changedEntity in ChangeTracker.Entries())
            {
                if (changedEntity.Entity is CMSEntity entity)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            if (string.IsNullOrEmpty(entity.Id))
                            {
                                entity.Id = Guid.NewGuid().ToString();
                            }
                            entity.CreatedAt = now;
                            entity.ModifiedAt = now;
                            break;
                        case EntityState.Modified:
                            Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                            entity.ModifiedAt = now;
                            break;
                    }
                }
                else if (changedEntity.Entity is AppUser user)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            // In case user somehow does not have an ID
                            if (string.IsNullOrEmpty(user.Id))
                            {
                                user.Id = Guid.NewGuid().ToString();
                            }
                            user.CreatedAt = now;
                            user.ModifiedAt = now;
                            break;
                        case EntityState.Modified:
                            Entry(user).Property(x => x.CreatedAt).IsModified = false;
                            user.ModifiedAt = now;
                            break;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            var now = DateTime.UtcNow;

            foreach (var changedEntity in ChangeTracker.Entries())
            {
                if (changedEntity.Entity is CMSEntity entity)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            if (string.IsNullOrEmpty(entity.Id))
                            {
                                entity.Id = Guid.NewGuid().ToString();
                            }
                            entity.CreatedAt = now;
                            entity.ModifiedAt = now;
                            break;
                        case EntityState.Modified:
                            Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                            entity.ModifiedAt = now;
                            break;
                    }
                }
                else if (changedEntity.Entity is AppUser user)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            // In case user somehow does not have an ID
                            if (string.IsNullOrEmpty(user.Id))
                            {
                                user.Id = Guid.NewGuid().ToString();
                            }
                            user.CreatedAt = now;
                            user.ModifiedAt = now;
                            break;
                        case EntityState.Modified:
                            Entry(user).Property(x => x.CreatedAt).IsModified = false;
                            user.ModifiedAt = now;
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }

        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppRoleClaim> AppRoleClaims { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppUserClaim> AppUserClaims { get; set; }
        public DbSet<AppUserLogin> AppUserLogins { get; set; }
        public DbSet<AppUserRole> AppUserRole { get; set; }
        public DbSet<AppUserToken> AppUserTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}