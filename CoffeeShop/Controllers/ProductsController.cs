using CoffeeShop.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Models.Services;

namespace CoffeeShop.Controllers
{
    public class ProductsController : Controller
    {
        private IProductRepository productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IActionResult Shop()
        {
            return View(productRepository.GetAllProducts());
        }
    }
}
