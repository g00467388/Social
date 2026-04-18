
public class PostRequest
{
    public string Title = string.Empty;
    public string Body = string.Empty;

    public PostRequest(string title, string body)
    {
        this.Title = title;
        this.Body = body;
    }
}