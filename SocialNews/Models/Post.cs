namespace SocialNews.Models
{
    public class Post
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public DateTime DateCreated { get; set; }
        public int Votes { get; set; }
        public ApplicationUser Author { get; set; }
        public List<Comment> Comments { get; set; } = new();
    }
}
