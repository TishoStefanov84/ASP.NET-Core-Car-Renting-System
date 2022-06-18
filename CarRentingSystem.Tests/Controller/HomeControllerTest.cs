namespace CarRentingSystem.Tests.Controller
{
    using CarRentingSystem.Controllers;
    using CarRentingSystem.Data.Models;
    using CarRentingSystem.Models.Home;
    using CarRentingSystem.Services.Cars;
    using CarRentingSystem.Services.Statistics;
    using CarRentingSystem.Tests.Mocks;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using MyTested.AspNetCore.Mvc;
    using Xunit;
    using System.Collections.Generic;
    using FluentAssertions;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyController<HomeController>
            .Instance(controller => controller
            .WithData(GetCars()))
            .Calling(c => c.Index())
            .ShouldReturn()
            .View(view => view
            .WithModelOfType<IndexViewModel>()
            .Passing(m => m.Cars.Should().HaveCount(3)));

        [Fact]
        public void IndexShouldReturnViewWithCorrectModel()
        {
            // Arrange
            var data = DatebaseMock.Instance;
            var mapper = MapperMock.Instance;

            var cars = GetCars();

            data.Cars.AddRange(cars);
            data.Users.Add(new User());

            data.SaveChanges();

            var carService = new CarService(data, mapper);
            var statisticService = new StatisticsService(data);

            var homeController = new HomeController(carService, statisticService);

            // Act
            var result = homeController.Index();

            //Assert
            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<IndexViewModel>(model);

            Assert.Equal(3, indexViewModel.Cars.Count);
            Assert.Equal(10, indexViewModel.TotalCars);
            Assert.Equal(1, indexViewModel.TotalUsers);
        }
        [Fact]
        public void ErrorShouldReturnView()
        {
            // Arrange
            var homeController = new HomeController(null, null);

            // Act
            var result = homeController.Error();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        private static IEnumerable<Car> GetCars()
            => Enumerable
                .Range(0, 10)
                .Select(i => new Car());
    }
}
