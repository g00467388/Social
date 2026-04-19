
public record PostRequest : Dto
{
    public string Title {get; set;} = string.Empty;
    public string Body {get; set;} = string.Empty;

    public PostRequest(string title, string body)
    {
        this.Title = title;
        this.Body = body;
    }
    public PostRequest()
    {
        
    }
}