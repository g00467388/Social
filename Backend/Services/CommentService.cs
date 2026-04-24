using Microsoft.EntityFrameworkCore;

public class CommentService : IService<Comment>
{

    private readonly ApplicationDbContext _context; 
    public CommentService(ApplicationDbContext context)
    {
        _context = context;
    }
     public async Task<IEnumerable<Comment>> GetAllAsync()
        => await _context.Comments.ToListAsync();

    public async Task<Comment?> GetByIdAsync(Guid id)
        => _context.Comments.FirstOrDefault(x => x.ID == id);

    public async Task<Comment?> InsertAsync(Comment item)
    {
        try
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
           Console.WriteLine("Failed to save {id}: {error} " + item.ID +  e.Message);
            return null;
        }
        return item;
    }

     public async Task<bool> DeleteItemByID(Guid id)
    {
        try
        {
            Comment? result = await _context.Comments.FirstOrDefaultAsync(x => x.ID == id);
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