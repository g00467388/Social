public record CommentRequest : Dto
{
    public string Content; 
    public Guid PostID;

    public CommentRequest(string content, Guid postId)
    {
        Content = content; 
        PostID = postId;
    }
    public CommentRequest()
    {
        
    }
}