using Microsoft.EntityFrameworkCore;
using BloggingSiteCMS.WebAPI.Controllers;
using BloggingSiteCMS.DAL;
using BloggingSiteCMS.DAL.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

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
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Cookie", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Cookie authorization",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddDbContext<CMSContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
});

// builder.Services.AddIdentity<AppUser, AppRole>();
builder.Services.AddIdentity<AppUser, AppRole>(o =>
{
    o.User.RequireUniqueEmail = true;

    o.Stores.MaxLengthForKeys = 128;

    // Default
    o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    o.Lockout.MaxFailedAccessAttempts = 5;
    o.Lockout.AllowedForNewUsers = true;
    // Default
    o.Password.RequireDigit = true;
    o.Password.RequireUppercase = true;
    o.Password.RequireNonAlphanumeric = true;
    o.Password.RequireLowercase = true;
    o.Password.RequiredLength = 6;
    o.Password.RequiredUniqueChars = 1;
})
.AddApiEndpoints()
.AddEntityFrameworkStores<CMSContext>()
.AddDefaultTokenProviders();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,

        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(28);
    options.LoginPath = "/Account/Login";
    // ReturnUrlParameter requires 
    //using Microsoft.AspNetCore.Authentication.Cookies;
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();

    using var serviceScope = app.Services.CreateScope();
    var context = serviceScope.ServiceProvider.GetService<CMSContext>();
    context?.Database.Migrate();
}
else
{
    app.UseHsts();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
