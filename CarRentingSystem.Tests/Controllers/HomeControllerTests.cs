namespace CarRentingSystem.Tests.Controllers
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using CarRentingSystem.Controllers;
    using System.Collections.Generic;
    using CarRentingSystem.Models.Home;
    using FluentAssertions;

    using static Data.Cars;
    using static WebConstatnts.Cache;

    public class HomeControllerTests
    {
        [Fact]
        public void IndexActionShouldReturnCorrectViewWithModel()
            => MyController<HomeController>
            .Instance(controller => controller
                .WithData(TenPublicCars))
            .Calling(c => c.Index())
            .ShouldHave()
            .MemoryCache(cache => cache
                .ContainingEntryWithKey(LatestCarsCacheKey))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<List<LatestCarServiceModel>>()
                .Passing(model => model.Should().HaveCount(3)));

        [Fact]
        public void ErrorShouldReturnView()
            => MyController<HomeController>
            .Instance()
            .Calling(c => c.Error())
            .ShouldReturn()
            .View();
    }
}
