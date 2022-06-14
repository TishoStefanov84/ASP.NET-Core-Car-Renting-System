﻿namespace CarRentingSystem.Controllers
{
    using CarRentingSystem.Infrastructure;
    using CarRentingSystem.Models.Cars;
    using CarRentingSystem.Services.Cars;
    using CarRentingSystem.Services.Dealers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CarsController : Controller
    {
        private readonly ICarService cars;
        private readonly IDealerService dealers;

        public CarsController(ICarService cars, IDealerService dealers)
        {
            this.cars = cars;
            this.dealers = dealers;
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

            this.cars.Create(
                car.Brand,
                car.Model,
                car.Description,
                car.ImageUrl,
                car.Year,
                car.CategoryId,
                dealerId);

            return RedirectToAction(nameof(All));
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

            return View(new CarFormModel
            {
                Brand = car.Brand,
                Model = car.Model,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                Year = car.Year,
                CategoryId = car.CategoryId,
                Categoies = this.cars.AllCarCategories()
            });
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

            this.cars.Edit(
                id,
                car.Brand,
                car.Model,
                car.Description,
                car.ImageUrl,
                car.Year,
                car.CategoryId);

            return RedirectToAction(nameof(All));
        }

    }
}
