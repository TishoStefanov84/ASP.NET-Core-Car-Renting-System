namespace CarRentingSystem.Services.Cars
{
    using CarRentingSystem.Models;
    using CarRentingSystem.Models.Home;
    using CarRentingSystem.Services.Cars.Models;
    using System.Collections.Generic;

    public interface ICarService
    {
        CarQueryServiceModel All(
            string brand = null,
            string searchTerm = null,
            CarSorting sorting = CarSorting.DateCreated,
            int currentPage = 1,
            int carsPerPage = int.MaxValue,
            bool publicOnly = true);

        IEnumerable<LatestCarServiceModel> Latest();

        CarDetailsServiceModel Details(int carId);

        int Create(
            string brand,
            string model,
            string description,
            string imageUrl,
            int year,
            int categoryId,
            int dealerId);

        bool Edit(
            int carId,
            string brand,
            string model,
            string description,
            string imageUrl,
            int year,
            int categoryId, 
            bool isPublic);

        IEnumerable<CarServiceModel> ByUser(string userId);

        bool IsByDealer(int carId, int dealerId);

        void ChangeVisibility(int id);

        IEnumerable<string> AllCarBrands();

        IEnumerable<CarCategoryServiceModel> AllCarCategories();

        bool CategoryExists(int categoryId);
    }
}
