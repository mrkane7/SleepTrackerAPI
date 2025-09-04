public interface ISleepUseCase
{
    Task<Result<IEnumerable<SleepDTO>>> GetAllSleep();
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