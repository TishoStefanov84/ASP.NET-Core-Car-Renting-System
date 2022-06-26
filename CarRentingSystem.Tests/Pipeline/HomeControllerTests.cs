namespace CarRentingSystem.Tests.Pipeline
{
    using CarRentingSystem.Controllers;
    using CarRentingSystem.Models.Home;
    using FluentAssertions;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using Xunit;

    using static Data.Cars;
    using static WebConstatnts.Cache;

    public class HomeControllerTests
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyMvc
            .Pipeline()
            .ShouldMap("/")
            .To<HomeController>(c => c.Index())
            .Which(controller => controller
                .WithData(TenPublicCars))
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<List<LatestCarServiceModel>>()
                .Passing(m => m.Should().HaveCount(3)));

        [Fact]

        public void ErrorShouldReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error())
                .Which()
                .ShouldReturn()
                .View();

      
    }
}
