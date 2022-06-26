namespace CarRentingSystem.Tests.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using CarRentingSystem.Controllers;

    public class HomeControllerTests
    {
        [Fact]
        public void IndexRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/")
            .To<HomeController>(c => c.Index());

        [Fact]
        public void ErrorRouteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Home/Error")
            .To<HomeController>(c => c.Error());
    }
}
