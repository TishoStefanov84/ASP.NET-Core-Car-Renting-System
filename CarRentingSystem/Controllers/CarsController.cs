namespace CarRentingSystem.Controllers
{
    using AutoMapper;
    using CarRentingSystem.Infrastructure.Extensions;
    using CarRentingSystem.Models.Cars;
    using CarRentingSystem.Services.Cars;
    using CarRentingSystem.Services.Dealers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static WebConstatnts;

    public class CarsController : Controller
    {
        private readonly ICarService cars;
        private readonly IDealerService dealers;
        private readonly IMapper mapper;

        public CarsController(
            ICarService cars, 
            IDealerService dealers, 
            IMapper mapper)
        {
            this.cars = cars;
            this.dealers = dealers;
            this.mapper = mapper;
        }
        public IActionResult All([FromQuery] AllCarsQueryModel query)
        {
            var queryResult = this.cars.All(
                query.Brand,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllCarsQueryModel.CarsPerPage);

            var carBrands = this.cars.AllCarBrands();

            query.TotalCars = queryResult.TotalCars;
            query.Brands = carBrands;
            query.Cars = queryResult.Cars;

            return View(query);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var myCars = this.cars.ByUser(this.User.GetId());

            return View(myCars);
        }

        public IActionResult Details(int id, string information)
        {
            var car = this.cars.Details(id);

            if (information != car.GetInformation())
            {
                return BadRequest();
            }

            return View(car);
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.dealers.IsDealer(this.User.GetId()))
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            return View(new CarFormModel
            {
                Categoies = this.cars.AllCarCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(CarFormModel car)
        {
            var dealerId = this.dealers.GetIdByUser(this.User.GetId());

            if (dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            if (!this.cars.CategoryExists(car.CategoryId))
            {
                this.ModelState.AddModelError(nameof(car.CategoryId), "Category dose not exist.");
            }

            if (!ModelState.IsValid)
            {
                car.Categoies = this.cars.AllCarCategories();

                return View(car);
            }

            var carId = this.cars.Create(
                car.Brand,
                car.Model,
                car.Description,
                car.ImageUrl,
                car.Year,
                car.CategoryId,
                dealerId);

            TempData[GlobalMessageKey] = "Your car was added and is waiting for approval!";

            return RedirectToAction(nameof(Details), new { id = carId, information = car.GetInformation() });
        }

           
        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetId();

            if (!this.dealers.IsDealer(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            var car = this.cars.Details(id);

            if (car.UserId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            var carForm = this.mapper.Map<CarFormModel>(car);

            carForm.Categoies = this.cars.AllCarCategories();

            return View(carForm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, CarFormModel car)
        {
            var dealerId = this.dealers.GetIdByUser(this.User.GetId());

            if (dealerId == 0 && !User.IsAdmin())
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            if (!this.cars.CategoryExists(car.CategoryId) && !User.IsAdmin())
            {
                this.ModelState.AddModelError(nameof(car.CategoryId), "Category dose not exist.");
            }

            if (!ModelState.IsValid)
            {
                car.Categoies = this.cars.AllCarCategories();

                return View(car);
            }

            if (!this.cars.IsByDealer(id, dealerId))
            {
                return BadRequest();
            }

            var edited = this.cars.Edit(
                id,
                car.Brand,
                car.Model,
                car.Description,
                car.ImageUrl,
                car.Year,
                car.CategoryId,
                this.User.IsAdmin());

            if (!edited)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = $"Your car was edited{(this.User.IsAdmin() ? string.Empty : "and is waiting for approval")}!";

            return RedirectToAction(nameof(Details), new { id, information = car.GetInformation() });
        }

    }
}
