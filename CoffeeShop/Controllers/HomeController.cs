using CoffeeShop.Models;
using CoffeeShop.Models.Interfaces;
using CoffeeShop.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CoffeeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IProductRepository ProductRepository;
        public HomeController(ILogger<HomeController> logger, IProductRepository ProductRepository)
        {
            _logger = logger;
            this.ProductRepository = ProductRepository;
        }

        public IActionResult Index()
        {
            return View(ProductRepository.GetTrendingProducts());
        }

    }
}
