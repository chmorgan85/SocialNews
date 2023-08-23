namespace SocialNews.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public int Votes { get; set; }
        public DateTime DateCreated { get; set; }
        public ApplicationUser Author { get; set; }
        public Post Post { get; set; }
    }
}
