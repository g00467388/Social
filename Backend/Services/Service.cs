using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

public class Service<T> : IService<T> where T : Dto
{
   // private readonly ILogger _logger;
    private readonly DbContext _context;
    private readonly DbSet<T> _set;
    public Service(ApplicationDbContext context)
    {
       // _logger = logger;
        _context = context;

        // assign supplied generic as set to query
        _set = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _set.ToListAsync();

    public async Task<T?> GetByIdAsync(Guid id)
        => _set.FirstOrDefault(x => x.ID == id);

    public async Task<T?> InsertAsync(T item)
    {
        try
        {
            await _set.AddAsync(item);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
           Console.WriteLine("Failed to save {typename}: {error} " + typeof(T).FullName +  e.Message);
            return null;
        }
        return item;
    }
    public async Task<bool> DeleteItemByID(Guid id)
    {
        try
        {
            T? result = await _set.FirstOrDefaultAsync(x => x.ID == id);
            if (result == null)
                return false;
            _set.Remove(result);
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