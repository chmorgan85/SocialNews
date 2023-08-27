namespace SocialNews.Models
{
    public class Post
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int Upvotes { get; set; } = 1;

        public ApplicationUser? Author { get; set; }
        public List<Comment> Comments { get; set; } = new();

        public List<ApplicationUser> UsersUpvoting { get; set; } = new();
        public List<ApplicationUser> UsersSaving { get; set; } = new();
    }
}
