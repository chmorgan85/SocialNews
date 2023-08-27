using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNews.Models;

namespace SocialNews.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

			builder.Entity<Post>()
				.HasOne(p => p.Author)
				.WithMany(u => u.Posts);
			builder.Entity<Post>()
				.HasMany(p => p.Comments)
				.WithOne(c => c.Post);
			builder.Entity<Post>()
				.HasMany(p => p.UsersUpvoting)
				.WithMany(u => u.UpvotedPosts);
			builder.Entity<Post>()
				.HasMany(p => p.UsersSaving)
				.WithMany(u => u.SavedPosts);

			builder.Entity<Comment>()
				.HasOne(c => c.Author)
				.WithMany(u => u.Comments);
			builder.Entity<Comment>()
				.HasOne(c => c.Post)
				.WithMany(p => p.Comments);
			builder.Entity<Comment>()
				.HasMany(c => c.UsersUpvoting)
				.WithMany(u => u.UpvotedComments);
			
			builder.Entity<ApplicationUser>()
				.HasMany(u => u.Posts)
				.WithOne(p => p.Author);
			builder.Entity<ApplicationUser>()
				.HasMany(u => u.Comments)
				.WithOne(c => c.Author);
			builder.Entity<ApplicationUser>()
				.HasMany(u => u.UpvotedPosts)
				.WithMany(p => p.UsersUpvoting);
			builder.Entity<ApplicationUser>()
				.HasMany(u => u.UpvotedComments)
				.WithMany(c => c.UsersUpvoting);
			builder.Entity<ApplicationUser>()
				.HasMany(u => u.SavedPosts)
				.WithMany(p => p.UsersSaving);
        }

        public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
	}
}