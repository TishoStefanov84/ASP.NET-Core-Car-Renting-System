namespace CarRentingSystem.Tests.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using CarRentingSystem.Controllers;
    using CarRentingSystem.Models.Dealers;

    public class DealersControllerTests
    {
        [Fact]
        public void GetBecomeShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Dealers/Become")
            .To<DealersController>(c => c.Become());

        [Fact]
        public void PostBecomeShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Dealers/Become")
                .WithMethod(HttpMethod.Post))
            .To<DealersController>(c => c.Become(With.Any<BecomeDealerFormModel>()));

    }
}
