using Microsoft.EntityFrameworkCore;
using BloggingSiteCMS.WebAPI.Controllers;
using BloggingSiteCMS.DAL;
using BloggingSiteCMS.DAL.Domain;

var builder = WebApplication.CreateBuilder(args);

// Custom configuration, grabs things like Connection Strings, which gets overwritten (if exists) in order of the chained method
// appsettings.json -> appsettings.{env}.json -> env variables (in this case docker) -> user secrets
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddDbContext<CMSContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityCore<AppUser>().AddEntityFrameworkStores<CMSContext>();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,

        policy =>
        {
            policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        });
});

// Add repositories to scope
builder.Services.AddScoped<TestController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var serviceScope = app.Services.CreateScope();
    var context = serviceScope.ServiceProvider.GetService<CMSContext>();
    context?.Database.Migrate();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
