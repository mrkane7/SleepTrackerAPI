using System.Collections.Immutable;
using AutoFixture;
using Moq;


namespace SleepTrackerAPITests
{
    [TestClass]
    public class SleepControllerTests
    {
        private Mock<ISleepService> _sleepService;
        private Fixture _fixture;

        public SleepControllerTests()
        {
            _fixture = new Fixture();
            _sleepService = new Mock<ISleepService>();
        }

        [TestMethod]
        public async Task Get_All_Sleep_ReturnOk()
        {
            var sleepList = _fixture.CreateMany<SleepDTO>(3).ToImmutableArray();

            _sleepService.Setup(service => service.GetAllSleepAsync()).ReturnsAsync(sleepList);

            var useCase = new SleepUseCase(_sleepService.Object);

            var result = await useCase.GetAllSleep();

            Assert.AreEqual(result.Successful, true);
            Assert.AreEqual(result.GetResult(x => x), sleepList);
        }

        [TestMethod]
        public async Task Get_User_Sleep_ReturnOk()
        {
            var sleepList = _fixture.CreateMany<SleepDTO>(1).ToImmutableArray();

            _sleepService.Setup(service => service.GetUsersSleep(sleepList[0].Id)).ReturnsAsync(sleepList);

            var useCase = new SleepUseCase(_sleepService.Object);

            var result = await useCase.GetUsersSleep(sleepList[0].Id);

            Assert.AreEqual(result.Successful, true);
            Assert.AreEqual(result.GetResult(x => x), sleepList);
        }

        [TestMethod]
        public void Get_User_Stats_None()
        {
            var sleepStatshandler = new SleepStatsHandler();

            var sleepGoal = 8;
            var sleepArray = new[]
            {
                // 8 Hours
                new SleepDTO { Id = 1, UserId = 101, StartTime = DateTime.Today.AddDays(-2).AddHours(22), EndTime = DateTime.Today.AddDays(-1).AddHours(6), Quality = 83 },
                // 8 Hours
                new SleepDTO { Id = 2, UserId = 101, StartTime = DateTime.Today.AddDays(-1).AddHours(23), EndTime = DateTime.Today.AddHours(7), Quality = 94 },
                // 7 Hours
                new SleepDTO { Id = 3, UserId = 101, StartTime = DateTime.Today.AddHours(0), EndTime = DateTime.Today.AddHours(7), Quality = 89 }
            };


            var sleepStats = sleepStatshandler.GenerateSleepStats(sleepArray, sleepGoal);
            var expectedSleepStats = new SleepStatsDTO
            {
                UserId = 101,
                WeeklyAverage = 7.666666666666667,
                MonthlyAverage = 7.666666666666667,
                YearlyAverage = 7.666666666666667,
                WeeklyDebt = 33,
                MonthlyDebt = 217,
                YearlyDebt = 2897
            };

            Assert.AreEqual(expectedSleepStats.UserId, sleepStats?.UserId);
            Assert.AreEqual(expectedSleepStats.WeeklyAverage, sleepStats?.WeeklyAverage);
            Assert.AreEqual(expectedSleepStats.MonthlyAverage, sleepStats?.MonthlyAverage);
            Assert.AreEqual(expectedSleepStats.YearlyAverage, sleepStats?.YearlyAverage);
            Assert.AreEqual(expectedSleepStats.WeeklyDebt, sleepStats?.WeeklyDebt);
            Assert.AreEqual(expectedSleepStats.MonthlyDebt, sleepStats?.MonthlyDebt);
            Assert.AreEqual(expectedSleepStats.YearlyDebt, sleepStats?.YearlyDebt);
        }
    }
}
