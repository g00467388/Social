using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) :base(context)
    {
        
    }
    public DbSet<Post> Posts {get; set;} = null!;
    public DbSet<Comment> Comments {get; set;} = null!;
}