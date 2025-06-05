using CoffeeShop.Models;
using CoffeeShop.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private IOrderRepository orderRepository;
        private IShoppingCartRepository shoppingCartRepository;
        private readonly UserManager<IdentityUser> userManager;
        public OrdersController(IOrderRepository oderRepository,
                                IShoppingCartRepository shoppingCartRepossitory, 
                                UserManager<IdentityUser> userManager)
        {
            this.orderRepository = oderRepository;
            this.shoppingCartRepository = shoppingCartRepossitory;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Checkout()
        {
            var shoppingCartItems = shoppingCartRepository.GetAllShoppingCartItems();
            if (!shoppingCartItems.Any()) 
            {
                TempData["ErrorMessage"] = "Your shopping cart is empty. Please add some items before checking out.";
                return RedirectToAction("Index", "ShoppingCart"); 
            }
            return View(new Order());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order) 
        {
            var shoppingCartItems = shoppingCartRepository.GetAllShoppingCartItems();

            if (!shoppingCartItems.Any())
            {
                ModelState.AddModelError("", "Your cart is empty, please add some products.");
                return View(order);
            }

            var user = await userManager.GetUserAsync(User); 
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            order.UserId = user.Id;

            order.OrderTotal = shoppingCartRepository.GetShoppingCartTotal();

            if (ModelState.IsValid)
            {
                try
                {
                    orderRepository.PlaceOrder(order);

                    shoppingCartRepository.ClearCart();
                    HttpContext.Session.SetInt32("CartCount", 0);

                    TempData["OrderSuccessMessage"] = "Your order has been placed successfully!";
                    return RedirectToAction("CheckoutComplete");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "There was an error placing your order. Please try again.");
                    TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                    return View(order);
                }
            }
            return View(order);
        }
        public IActionResult CheckoutComplete()
        {
            ViewBag.OrderSuccessMessage = TempData["OrderSuccessMessage"];
            return View();
        }
        public async Task<IActionResult> ListOrders()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var userId = user.Id;
            var orders = await orderRepository.GetOrdersForUserAsync(userId);

            if (!orders.Any())
            {
                ViewBag.Message = "You haven't placed any orders yet.";
            }

            return View(orders);
        }
    }
}
