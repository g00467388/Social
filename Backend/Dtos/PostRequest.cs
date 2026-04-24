
public record PostRequest 
{
    public string Title {get; set;} = string.Empty;
    public string Body {get; set;} = string.Empty;

    public PostRequest(string title, string body)
    {
        Title = title;
        Body = body;
    }
    public PostRequest()
    {
        
    }
}