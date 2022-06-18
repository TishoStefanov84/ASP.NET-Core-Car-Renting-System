namespace CarRentingSystem.Tests.Mocks
{
    using CarRentingSystem.Services.Statistics;
    using Moq;

    public static class StatisticServiceMock
    {
        public static IStatisticsService Instance
        {
            get
            {
                var statisticServiceMock = new Mock<IStatisticsService>();

                statisticServiceMock
                    .Setup(s => s.Total())
                    .Returns(new StatisticsServiceModel
                    {
                        TotalCars = 5,
                        TotalRents = 10,
                        TotalUsers = 15
                    });

                return statisticServiceMock.Object;
            }
        }
    }
}
