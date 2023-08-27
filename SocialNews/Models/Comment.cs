namespace SocialNews.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public int Upvotes { get; set; } = 1;
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public ApplicationUser? Author { get; set; }
        public Post? Post { get; set; }

        public List<ApplicationUser> UsersUpvoting { get; set; } = new();
    }
}
