using Microsoft.IdentityModel.Tokens;

public class SleepStatsHandler
{
    public SleepStatsDTO? GenerateSleepStats(IEnumerable<SleepDTO> sleep, int sleepGoal)
    {
        if (sleep.IsNullOrEmpty())
        {
            return null;
        }
        var sleepId = sleep.FirstOrDefault()?.UserId;
        if (sleepId == null)
        {
            return null;
        }
        return new SleepStatsDTO {
            UserId = sleepId ?? 0,
            WeeklyAverage = sleep.Where(s => s.StartTime >= DateTime.Now.AddDays(-7)).Average(s => (s.EndTime - s.StartTime).TotalHours),
            MonthlyAverage = sleep.Where(s => s.StartTime >= DateTime.Now.AddDays(-30)).Average(s => (s.EndTime - s.StartTime).TotalHours),
            YearlyAverage = sleep.Where(s => s.StartTime >= DateTime.Now.AddDays(-365)).Average(s => (s.EndTime - s.StartTime).TotalHours),
            WeeklyDebt = (int)(7 * sleepGoal - sleep.Where(s => s.StartTime >= DateTime.Now.AddDays(-7)).Sum(s => (s.EndTime - s.StartTime).TotalHours)),
            MonthlyDebt = (int)(30 * sleepGoal - sleep.Where(s => s.StartTime >= DateTime.Now.AddDays(-30)).Sum(s => (s.EndTime - s.StartTime).TotalHours)),
            YearlyDebt = (int)(365 * sleepGoal - sleep.Where(s => s.StartTime >= DateTime.Now.AddDays(-365)).Sum(s => (s.EndTime - s.StartTime).TotalHours)),
        };
    }
}