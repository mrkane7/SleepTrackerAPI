using Microsoft.EntityFrameworkCore;

public interface ISleepService
{
    Task<IEnumerable<SleepDTO>> GetAllSleepAsync();
    Task<IEnumerable<SleepDTO>> GetUsersSleep(int userId);
    Task<SleepDTO> AddSleepAsync(SleepDTO sleep);
    Task<SleepDTO> EditSleep(SleepDTO sleep);
    Task<bool> DeleteSleep(int id);
}

public class SleepService : ISleepService
{
    private readonly ApplicationDbContext _context;

    public SleepService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SleepDTO>> GetAllSleepAsync()
    {
        return await _context.Sleep.ToListAsync();
    }
    public async Task<IEnumerable<SleepDTO>> GetUsersSleep(int userId)
    {
        return await _context.Sleep.Where(s => s.UserId == userId).ToListAsync();
    }
    public async Task<SleepDTO> AddSleepAsync(SleepDTO sleep)
    {
        _context.Sleep.Add(sleep);
        await _context.SaveChangesAsync();
        return sleep;
    }
    public async Task<SleepDTO> EditSleep(SleepDTO sleep)
    {
        _context.Sleep.Update(sleep);
        await _context.SaveChangesAsync();
        return sleep;
    }
    public async Task<bool> DeleteSleep(int id)
    {
        var sleep = _context.Sleep.Where(s => s.Id == id).FirstOrDefault();
        if (sleep != null)
        {
            _context.Sleep.Remove(sleep);
        }
        else
        {
            return false;
        }
        await _context.SaveChangesAsync();
        return true;
    }

}