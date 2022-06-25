namespace CarRentingSystem.Controllers
{
    using CarRentingSystem.Models.Home;
    using CarRentingSystem.Services.Cars;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class HomeController : Controller
    {
        private readonly ICarService cars;
        private readonly IMemoryCache cache;

        public HomeController(
            ICarService cars,
            IMemoryCache cache)
        {
            this.cars = cars;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            const string latestCarsCacheKey = "LatestCarsCacheKey";

            var latestCars = this.cache.Get<List<LatestCarServiceModel>>(latestCarsCacheKey);

            if (latestCars == null)
            {
                latestCars = this.cars.Latest().ToList();
            
                var cacheOptionas = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
            
                this.cache.Set(latestCarsCacheKey, latestCars, cacheOptionas);
            }

            return View(latestCars);
        }

        public IActionResult Error() 
            => View();
    }
}
