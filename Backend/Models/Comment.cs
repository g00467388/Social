public record Comment : CommentRequest
{
    public DateTime CreationDate;

    public Comment(string content, Guid postId) :base(content, postId)
    {
        CreationDate = DateTime.UtcNow;
    }
    public Comment()
    {
        
    }

}