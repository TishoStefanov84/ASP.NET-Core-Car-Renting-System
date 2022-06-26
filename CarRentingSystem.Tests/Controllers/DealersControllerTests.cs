namespace CarRentingSystem.Tests.Controllers
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using CarRentingSystem.Controllers;
    using CarRentingSystem.Models.Dealers;
    using CarRentingSystem.Data.Models;
    using System.Linq;

    using static WebConstatnts;
    using CarRentingSystem.Models.Cars;

    public class DealersControllerTests
    {
        [Fact]
        public void GetBecomeShouldBeForAuthorizedUsers()
            => MyController<DealersController>
            .Instance()
            .Calling(c => c.Become())
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void GetBecomeShouldReturnView()
            => MyController<DealersController>
            .Instance()
            .Calling(c => c.Become())
            .ShouldReturn()
            .View();


        [Theory]
        [InlineData("DealerName", "+359888888888")]
        public void PostBecomeShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(
            string dealerName,
            string phoneNumber)
            => MyController<DealersController>
            .Instance(controller => controller
                .WithUser())
            .Calling(c => c.Become(new BecomeDealerFormModel
            {
                Name = dealerName,
                PhoneNumber = phoneNumber
            }))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .RestrictingForAuthorizedRequests())
            .ValidModelState()
            .Data(data => data
                .WithSet<Dealer>(dealers => dealers
                .Any(d =>
                        d.Name == dealerName &&
                        d.PhoneNumber == phoneNumber &&
                        d.UserId == TestUser.Identifier)))
            .TempData(tempData => tempData
                .ContainingEntryWithKey(GlobalMessageKey))
            .AndAlso()
            .ShouldReturn()
            .Redirect(redirect => redirect
                .To<CarsController>(c => c.All(With.Any<AllCarsQueryModel>())));
    }
}
