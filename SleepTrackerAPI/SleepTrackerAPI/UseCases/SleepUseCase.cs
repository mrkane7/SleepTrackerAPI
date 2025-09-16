using Microsoft.IdentityModel.Tokens;

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
    private readonly SleepStatsHandler _sleepStatsHandler;

    public SleepUseCase(ISleepService sleepService)
    {
        _sleepService = sleepService;
        _sleepStatsHandler = new SleepStatsHandler();
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
        var sleepStats = _sleepStatsHandler.GenerateSleepStats(userSleep, sleepGoal);

        if (sleepStats == null)
        {
            return Result<SleepStatsDTO>.Fail(ResultReason.NotFound, "No sleep stats found.");
        }

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