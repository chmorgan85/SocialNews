using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNews.Data;
using SocialNews.Models;
using SocialNews.Models.ViewModels;

namespace SocialNews.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string username, string view = "posts")
        {
            var user = context.Users.SingleOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return NotFound();
            }

            var usersVM = new UsersViewModel
            {
                UserName = username,
                View = view
            };

            switch (view)
            {
                case "posts":
                    usersVM.Posts = context.Posts
                        .Include(p => p.Author)
                        .Include(p => p.Comments)
                        .Include(p => p.UsersUpvoting)
                        .Include(p => p.UsersSaving)
                        .Where(p => p.Author.UserName == username)
                        .ToList();
                    break;

                case "comments":
                    usersVM.Comments = context.Comments
                        .Include(c => c.Author)
                        .Include(c => c.Post)
                        .Include(c => c.UsersUpvoting)
                        .Where(c => c.Author.UserName == username)
                        .ToList();
                    break;

                case "saved":
                    user = await userManager.GetUserAsync(User);
                    usersVM.Posts = context.Posts
						.Include(p => p.Author)
						.Include(p => p.Comments)
						.Include(p => p.UsersUpvoting)
						.Include(p => p.UsersSaving)
						.Where(p => p.UsersSaving.Contains(user))
						.ToList();
                    break;
			}

            return View(usersVM);
        }
    }
}
