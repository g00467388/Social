public class Post : PostRequest
{
    public Guid ID;
    public DateTime CreationDate;

    public Post(string title, string body) : base(title, body)
    {
        // ID Created by entityframework upon storing
        CreationDate = DateTime.UtcNow;
    }
}