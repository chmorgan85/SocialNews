using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNews.Data;
using SocialNews.Models;
using SocialNews.Models.ViewModels;

namespace SocialNews.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IActionResult Index(int id, string sort = "top")
        {
            var post = context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments).ThenInclude(c => c.Author)
                .Include(p => p.Comments).ThenInclude(c => c.UsersUpvoting)
                .Include(p => p.UsersUpvoting)
                .Include(p => p.UsersSaving)
                .SingleOrDefault(p => p.ID == id);

            if (post == null)
            {
                return NotFound();
            }

            switch (sort)
            {
                case "top":
                    post.Comments.Sort((x, y) => y.Upvotes.CompareTo(x.Upvotes));
                    break;
                case "new":
                    post.Comments.Sort((x, y) => y.DateCreated.CompareTo(x.DateCreated));
                    break;
            }

            return View(new CommentsViewModel
            {
                Post = post,
                Sort = sort
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(int postID, string content)
        {
            var post = context.Posts
                .Include(p => p.Comments)
                .SingleOrDefault(p => p.ID == postID);

            if (post == null)
            {
                return NotFound();
            }

            var user = await userManager.GetUserAsync(User);
            var comment = new Comment
            {
                Content = content,
                Author = user,
                Post = post
            };
            comment.UsersUpvoting.Add(user);

            post.Comments.Add(comment);
            context.SaveChanges();

            return RedirectToAction(nameof(Index), new { id = postID });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upvote(int id)
        {
            var comment = context.Comments
                .Include(c => c.UsersUpvoting)
                .SingleOrDefault(c => c.ID == id);

            if (comment == null)
            {
                return NotFound();
            }

            comment.UsersUpvoting.Add(await userManager.GetUserAsync(User));
            comment.Upvotes++;
            context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUpvote(int id)
        {
            var comment = context.Comments
                .Include(c => c.UsersUpvoting)
                .SingleOrDefault(c => c.ID == id);

            if (comment == null)
            {
                return NotFound();
            }

            var success = comment.UsersUpvoting.Remove(await userManager.GetUserAsync(User));
            if (!success)
            {
                return BadRequest();
            }

            comment.Upvotes--;
            context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string content)
        {
            var comment = context.Comments
                .Include(c => c.Author)
                .Include(c => c.Post)
                .SingleOrDefault(c => c.ID == id);

            if (comment == null)
            {
                return NotFound();
            }

            if (comment.Author != await userManager.GetUserAsync(User))
            {
                return Unauthorized();
            }

            comment.Content = content;
            context.SaveChanges();

            return RedirectToAction(nameof(Index), new { id = comment.Post.ID });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = context.Comments
                .Include(c => c.Author)
                .Include(c => c.Post)
                .SingleOrDefault(p => p.ID == id);

            if (comment == null)
            {
                return NotFound();
            }

            if (comment.Author != await userManager.GetUserAsync(User))
            {
                return Unauthorized();
            }

            var postID = comment.Post.ID;

            context.Remove(comment);
            context.SaveChanges();

            return RedirectToAction(nameof(Index), new { id = postID });
        }
    }
}
