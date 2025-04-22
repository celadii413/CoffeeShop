using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Models.Services;

namespace CoffeeShop.Models.Interfaces
{
    public interface IProductRepository 
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetTrendingProducts();
        Product GetProductDetail(int id);
    }
}
