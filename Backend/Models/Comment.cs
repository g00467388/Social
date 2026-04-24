using System.ComponentModel.DataAnnotations;

public record Comment : CommentRequest
{
    [Key]
    public Guid ID {get; set;}
    public DateTime CreationDate;
    
    
    public string UserId {get; set;}
    public User User {get; set;}
    
    public Comment(string content, string postId) :base(content, postId)
    {
        CreationDate = DateTime.UtcNow;
    }
    public Comment()
    {
        
    }

}