using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNews.Data;
using SocialNews.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

/*
app.MapControllerRoute(
	name: "home",
	pattern: "{action=Index}/{id?}",
	defaults: new { controller = "Home" });
app.MapControllerRoute(
    name: "comments",
    pattern: "{controller}/{id?}",
    defaults: new { controller = "Comments", action = "Index" });
*/
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Posts}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
