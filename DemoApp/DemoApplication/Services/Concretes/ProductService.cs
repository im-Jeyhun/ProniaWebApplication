using DemoApplication.Database;
using DemoApplication.Services.Abstracts;

namespace DemoApplication.Services.Concretes
{
    public class ProductService : IProductService
    {
        private const int MIN_RANDOM_NUMBER = 10000;
        private const int MAX_RANDOM_NUMBER = 100000;
        private const string PREFIX = "SKU-";

        private readonly DataContext _dataContext;

        public ProductService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        private string GenerateNumber()
        {
            Random randomId = new Random();
            return randomId.Next(MIN_RANDOM_NUMBER, MAX_RANDOM_NUMBER).ToString();
        }

        public string GenerateSku()
        {
            var sku = $"{PREFIX}{GenerateNumber()}";

            while (_dataContext.Plants.Any(p => p.Sku == sku))
            {
                sku = $"{PREFIX}{GenerateNumber()}";
            }

            return sku;

        }
    }
}
