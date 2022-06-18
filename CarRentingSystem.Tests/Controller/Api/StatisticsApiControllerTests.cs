namespace CarRentingSystem.Tests.Controller.Api
{
    using CarRentingSystem.Controllers.Api;
    using CarRentingSystem.Tests.Mocks;
    using Xunit;

    public class StatisticsApiControllerTests
    {
        [Fact]
        public void GetStatisticsShouldReturnTotalStatistics()
        {
            // Arrange
            var statisticsController = new StatisticsApiController(StatisticServiceMock.Instance);

            // Act
            var result = statisticsController.GetStatistics();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.TotalCars);
            Assert.Equal(10, result.TotalRents);
            Assert.Equal(15, result.TotalUsers);

        }
    }
}
