using Microsoft.AspNetCore.Identity;

namespace SocialNews.Models
{
	public class ApplicationUser : IdentityUser
	{
		public List<Post> Posts { get; set; } = new();
		public List<Comment> Comments { get; set; } = new();

		public List<Post> UpvotedPosts { get; set; } = new();
		public List<Comment> UpvotedComments { get; set; } = new();
		public List<Post> SavedPosts { get; set; } = new();
	}
}
