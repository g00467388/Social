public record Post : PostRequest
{
    public DateTime CreationDate;

    public Post(string title, string body) : base(title, body)
    {
        // ID Created by entityframework upon storing
        CreationDate = DateTime.UtcNow;
    }
    public Post()
    {
        
    }
}