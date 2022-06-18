namespace CarRentingSystem.Tests.Mocks
{
    using CarRentingSystem.Data;
    using Microsoft.EntityFrameworkCore;
    using System;

    public static class DatebaseMock
    {
        public static CarRentingDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<CarRentingDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new CarRentingDbContext(dbContextOptions);
            }
        }
    }
}
