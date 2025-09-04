public interface ISleepUseCase
{
    Task<Result<IEnumerable<SleepDTO>>> GetAllSleep();
    Task<Result<IEnumerable<SleepDTO>>> GetUsersSleep(int userId);
    Task<Result<SleepStatsDTO>> GetUserSleepStats(int userId);
    Task<Result<SleepDTO>> AddSleep(SleepDTO sleep);
    Task<Result<SleepDTO>> EditSleep(SleepDTO sleep);
    Task<Result> DeleteSleep(int id);
}

public class SleepUseCase : ISleepUseCase
{
    private readonly ISleepService _sleepService;

    public SleepUseCase(ISleepService sleepService)
    {
        _sleepService = sleepService;
    }

    public async Task<Result<IEnumerable<SleepDTO>>> GetAllSleep()
    {
        var sleepData = await _sleepService.GetAllSleepAsync();
        return Result<IEnumerable<SleepDTO>>.Success(sleepData);
    }
    public async Task<Result<IEnumerable<SleepDTO>>> GetUsersSleep(int userId)
    {
        var sleepData = await _sleepService.GetUsersSleep(userId);
        return Result<IEnumerable<SleepDTO>>.Success(sleepData);
    }
    public async Task<Result<SleepStatsDTO>> GetUserSleepStats(int userId)
    {
        var sleepGoal = 8;
        var userSleep = await _sleepService.GetUsersSleep(userId);
        var sleepStats = new SleepStatsDTO {
            UserId = userId,
            WeeklyAverage = userSleep.Where(s => s.StartTime >= DateTime.Now.AddDays(-7)).Average(s => (s.EndTime - s.StartTime).TotalHours),
            MonthlyAverage = userSleep.Where(s => s.StartTime >= DateTime.Now.AddDays(-30)).Average(s => (s.EndTime - s.StartTime).TotalHours),
            YearlyAverage = userSleep.Where(s => s.StartTime >= DateTime.Now.AddDays(-365)).Average(s => (s.EndTime - s.StartTime).TotalHours),
            WeeklyDebt = (int)(7 * sleepGoal - userSleep.Where(s => s.StartTime >= DateTime.Now.AddDays(-7)).Sum(s => (s.EndTime - s.StartTime).TotalHours)),
            MonthlyDebt = (int)(30 * sleepGoal - userSleep.Where(s => s.StartTime >= DateTime.Now.AddDays(-30)).Sum(s => (s.EndTime - s.StartTime).TotalHours)),
            YearlyDebt = (int)(365 * sleepGoal - userSleep.Where(s => s.StartTime >= DateTime.Now.AddDays(-365)).Sum(s => (s.EndTime - s.StartTime).TotalHours)),
        };

        return Result<SleepStatsDTO>.Success(sleepStats);
    }
    public async Task<Result<SleepDTO>> AddSleep(SleepDTO sleep)
    {
        var sleepData = await _sleepService.AddSleepAsync(sleep);
        return Result<SleepDTO>.Success(sleepData);
    }
    public async Task<Result<SleepDTO>> EditSleep(SleepDTO sleep)
    {
        var sleepData = await _sleepService.EditSleep(sleep);
        return Result<SleepDTO>.Success(sleepData);
    }
    public async Task<Result> DeleteSleep(int id)
    {
        var success = await _sleepService.DeleteSleep(id);
        return success ? Result.Success() : Result.Fail(ResultReason.NotFound, "Sleep entry not found.");
    }
}