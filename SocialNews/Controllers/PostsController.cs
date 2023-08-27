using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNews.Data;
using SocialNews.Models.ViewModels;
using SocialNews.Models;
using Microsoft.EntityFrameworkCore;

namespace SocialNews.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IActionResult Index(string sort = "hot")
        {
            var query = context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .Include(p => p.UsersUpvoting)
                .Include(p => p.UsersSaving);

            var postsVM = new PostsViewModel
            {
                Sort = sort
            };

            switch (sort)
            {
                case "hot":
                    var posts = query.ToList();
                    posts.Sort((x, y) => (y.Upvotes / y.DateCreated.Ticks).CompareTo(x.Upvotes / x.DateCreated.Ticks));
                    postsVM.Posts = posts;
                    break;

                case "new":
                    postsVM.Posts = query.OrderByDescending(p => p.DateCreated).ToList();
                    break;

                case "top":
                    postsVM.Posts = query.OrderByDescending(p => p.Upvotes).ToList();
                    break;
            }

            return View(postsVM);
        }

        [Authorize]
        public IActionResult Submit()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(SubmitViewModel submitVM)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                var post = new Post
                {
                    Title = submitVM.Title,
                    URL = submitVM.URL,
                    Author = user
                };
                post.UsersUpvoting.Add(user);

                context.Add(post);
                context.SaveChanges();

                return RedirectToAction("Index", "Comments", new { id = post.ID });
            }

            return View(submitVM);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upvote(int postID)
        {
            var post = context.Posts
                .Include(p => p.UsersUpvoting)
                .SingleOrDefault(p => p.ID == postID);

            if (post == null)
            {
                return NotFound();
            }

            post.UsersUpvoting.Add(await userManager.GetUserAsync(User));
            post.Upvotes++;
            context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUpvote(int postID)
        {
            var post = context.Posts
                .Include(p => p.UsersUpvoting)
                .SingleOrDefault(p => p.ID == postID);

            if (post == null)
            {
                return NotFound();
            }

            var success = post.UsersUpvoting.Remove(await userManager.GetUserAsync(User));
            if (!success)
            {
                return BadRequest();
            }

            post.Upvotes--;
            context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(int postID)
        {
            var post = context.Posts
                .Include(p => p.UsersSaving)
                .SingleOrDefault(p => p.ID == postID);

            if (post == null)
            {
                return NotFound();
            }

            post.UsersSaving.Add(await userManager.GetUserAsync(User));
            context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unsave(int postID)
        {
            var post = context.Posts
                .Include(p => p.UsersSaving)
                .SingleOrDefault(p => p.ID == postID);

            if (post == null)
            {
                return NotFound();
            }

            var success = post.UsersSaving.Remove(await userManager.GetUserAsync(User));
            if (!success)
            {
                return BadRequest();
            }
            context.SaveChanges();

            return Ok();
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var post = context.Posts
                .Include(p => p.Author)
                .SingleOrDefault(p => p.ID == id);

            if (post == null)
            {
                return NotFound();
            }

            if (post.Author != await userManager.GetUserAsync(User))
            {
                return Unauthorized();
            }

            return View(new EditViewModel
            {
                postID = post.ID,
                Title = post.Title,
                URL = post.URL
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel editVM)
        {
            if (ModelState.IsValid)
            {
				var post = context.Posts
                    .Include(p => p.Author)
                    .SingleOrDefault(p => p.ID == editVM.postID);

				if (post == null)
				{
					return NotFound();
				}

				if (post.Author != await userManager.GetUserAsync(User))
				{
					return Unauthorized();
				}

                post.Title = editVM.Title;
                post.URL = editVM.URL;
                context.SaveChanges();

                return RedirectToAction("Index", "Comments", new { id = post.ID });
			}

            return View(editVM);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
			var post = context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .SingleOrDefault(p => p.ID == id);

			if (post == null)
			{
				return NotFound();
			}

			if (post.Author != await userManager.GetUserAsync(User))
			{
				return Unauthorized();
			}

            context.RemoveRange(post.Comments);
            context.Remove(post);
            context.SaveChanges();

			return RedirectToAction(nameof(Index));
        }

        public IActionResult Search(string query)
        {
			var posts = context.Posts
				.Include(p => p.Author)
				.Include(p => p.Comments)
				.Include(p => p.UsersUpvoting)
				.Include(p => p.UsersSaving)
				.Where(p => p.Title.ToLower().Contains(query.ToLower()))
				.ToList();

            return View("Index", new PostsViewModel
            {
                Sort = "",
                Posts = posts
            });
        }
    }
}
