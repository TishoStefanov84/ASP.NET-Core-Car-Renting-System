namespace CarRentingSystem.Controllers
{
    using CarRentingSystem.Models.Home;
    using CarRentingSystem.Services.Cars;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using static WebConstatnts.Cache;

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
            var latestCars = this.cache.Get<List<LatestCarServiceModel>>(LatestCarsCacheKey);

            if (latestCars == null)
            {
                latestCars = this.cars.Latest().ToList();
            
                var cacheOptionas = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
            
                this.cache.Set(LatestCarsCacheKey, latestCars, cacheOptionas);
            }

            return View(latestCars);
        }

        public IActionResult Error() 
            => View();
    }
}
