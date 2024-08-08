using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BloggingSiteCMS.DAL
{
    public class CMSContextFactory : IDesignTimeDbContextFactory<CMSContext>
    {
        public CMSContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddUserSecrets<CMSContextFactory>()
            .Build();

            var optionsBuilder = new DbContextOptionsBuilder<CMSContext>();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            return new CMSContext(optionsBuilder.Options);
        }
    }
}