public class Post
{
    public Guid ID { get; set; }
    public DateTime CreationDate { get; set; }

    public string Title { get; set; }
    public string Body { get; set; }

    public string UserID { get; set; }
    public User User { get; set; }

    public Post() {} // EF Core needs this

    public Post(string title, string body, string userID)
    {
        Title = title;
        Body = body;
        UserID = userID;
        CreationDate = DateTime.UtcNow;
    }
}
