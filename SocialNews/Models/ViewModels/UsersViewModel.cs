namespace SocialNews.Models.ViewModels
{
    public class UsersViewModel
    {
        public string UserName { get; set; }
        public List<Post>? Posts { get; set; }
        public List<Comment>? Comments { get; set; }
        public string View { get; set; }
    }
}
