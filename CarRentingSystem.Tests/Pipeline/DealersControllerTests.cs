namespace CarRentingSystem.Tests.Pipeline
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using CarRentingSystem.Controllers;
    using CarRentingSystem.Models.Dealers;
    using CarRentingSystem.Data.Models;
    using System.Linq;
    using CarRentingSystem.Models.Cars;

    using static WebConstatnts;

    public class DealersControllerTests
    {
        [Fact]
        public void GetBecomeShouldBeForAuthorizedUsersAndReturnView()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Dealers/Become")
                .WithUser())
            .To<DealersController>(c => c.Become())
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View();

        [Theory]
        [InlineData("DealerName", "+359888888888")]
        public void PostBecomeShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(
           string dealerName,
           string phoneNumber)
           => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Dealers/Become")
                .WithMethod(HttpMethod.Post)
                .WithFormFields(new
                {
                    Name = dealerName,
                    PhoneNumber = phoneNumber
                })
                .WithUser()
                .WithAntiForgeryToken())
           .To<DealersController>(c => c.Become(new BecomeDealerFormModel
           {
               Name = dealerName,
               PhoneNumber = phoneNumber
           }))
           .Which()
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
