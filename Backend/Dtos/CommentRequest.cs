public record CommentRequest 
{
    public string Content {get; set;} 
    public string PostID {get; set;}

    public CommentRequest(string content, string postId)
    {
        Content = content; 
        PostID = postId;
    }
    public CommentRequest()
    {
        
    }
}