using Microsoft.EntityFrameworkCore;

public class PostService : IService<Post>
{
    private readonly ApplicationDbContext _context;
    public PostService(ApplicationDbContext context)
    {
        _context = context;
    }
   public async Task<IEnumerable<Post>> GetAllAsync()
    => await _context.Posts
        .Include(p => p.User)
        .ToListAsync();


    public async Task<Post?> GetByIdAsync(Guid id)
        => _context.Posts.FirstOrDefault(x => x.ID == id);


    public async Task<Post?> InsertAsync(Post item)
    {
        try
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to save {id}: {error} " + item.ID + e.Message);
            return null;
        }
        return item;
    }

    public async Task<bool> DeleteItemByID(Guid id)
    {
        try
        {
            Post? result = await _context.Posts.FirstOrDefaultAsync(x => x.ID == id);
            if (result == null)
                return false;
            _context.Remove(result);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            //     _logger.LogWarning("Failed to remove {ID}: {error} ", id, e.Message);
            return false;
        }
        return true;
    }


}