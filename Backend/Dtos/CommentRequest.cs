public class CommentRequest
{
    public string Content; 
    public Post post;

    public CommentRequest(string content, Post post)
    {
        this.Content = content; 
        this.post = post;
    }
}