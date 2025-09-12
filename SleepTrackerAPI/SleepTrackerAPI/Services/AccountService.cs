
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public interface IAccountService
{
    Task<UserAccountDTO?> GetAccount(int id);
    Task<UserAccountDTO?> GetAccountByEmail(string email);
    Task<UserAccountDTO> CreateAccount(UserAccountDTO userAccount);
    Task<UserAccountDTO> EditAccount(UserAccountDTO userAccount);
    Task<UserAccountDTO?> UpdateSleepGoal(int id, int sleepGoal);
    Task<bool> DeleteAccount(int id);
}

public class AccountService : IAccountService
{
    private readonly ApplicationDbContext _context;

    public AccountService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserAccountDTO?> GetAccount(int id)
    {
        var userAccount = await _context.UserAccounts.FindAsync(id);
        return userAccount;
    }
    public async Task<UserAccountDTO?> GetAccountByEmail(string email)
    {
        var userAccount = await _context.UserAccounts.Where(a => a.Email == email).FirstOrDefaultAsync();
        return userAccount;
    }

    public async Task<UserAccountDTO> CreateAccount(UserAccountDTO userAccount)
    {
        _context.UserAccounts.Add(userAccount);
        await _context.SaveChangesAsync();
        return userAccount;
    }
    public async Task<UserAccountDTO> EditAccount(UserAccountDTO userAccount)
    {
        _context.UserAccounts.Update(userAccount);
        await _context.SaveChangesAsync();
        return userAccount;
    }
    public async Task<UserAccountDTO?> UpdateSleepGoal(int id, int sleepGoal)
    {
        Console.WriteLine("ID and Sleep Goal: " + id + ", " + sleepGoal);
        var userAccount = await _context.UserAccounts.FindAsync(id);
        if (userAccount == null)
        {
            return null;
        }
        userAccount.SleepGoal = sleepGoal;
        await _context.SaveChangesAsync();
        return userAccount;
    }

    public async Task<bool> DeleteAccount(int id)
    {
        var userAccount = _context.UserAccounts.Where(a => a.Id == id).FirstOrDefault();

        if (userAccount != null)
        {
            _context.UserAccounts.Remove(userAccount);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

}