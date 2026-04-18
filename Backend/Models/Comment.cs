using Backend.Models;

public class Comment : CommentRequest
{
    public Guid ID; 
    public DateTime CreationDate;

    public Comment(string content, Post post) :base(content, post)
    {
        CreationDate = DateTime.UtcNow;
    }
}