using Microsoft.AspNetCore.Identity;

namespace backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Post> Posts { get; set; } = [];
        public List<Comment> Comments { get; set; } = [];
    }
}
