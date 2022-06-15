namespace CarRentingSystem.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using CarRentingSystem.Data;
    using CarRentingSystem.Models;
    using CarRentingSystem.Models.Cars;
    using CarRentingSystem.Models.Home;
    using CarRentingSystem.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Linq;

    public class HomeController : Controller
    {
        private readonly IStatisticsService statistics;
        private readonly IMapper mapper;
        private readonly CarRentingDbContext data;

        public HomeController(
            IStatisticsService statistics,
            IMapper mapper,
            CarRentingDbContext data)
        {
            this.statistics = statistics;
            this.mapper = mapper;
            this.data = data;
        }

        public IActionResult Index()
        {
            var cars = this.data
                .Cars
                .OrderByDescending(c => c.Id)
                .ProjectTo<CarIndexViewModel>(this.mapper.ConfigurationProvider)
                .Take(3)
                .ToList();

            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel
            {
                TotalCars = totalStatistics.TotalCars,
                TotalUsers = totalStatistics.TotalUsers,
                Cars = cars
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() 
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
