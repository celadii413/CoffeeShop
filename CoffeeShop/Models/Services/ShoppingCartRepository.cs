using CoffeeShop.Data;
using CoffeeShop.Models.Interfaces;
using CoffeeShop.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Models.Services
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private CoffeeShopDbContext dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartRepository(CoffeeShopDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public List<ShoppingCartItem>? ShoppingCartItems { get; set; }
        public string? ShoppingCartId { set; get; }

        public static ShoppingCartRepository GetCart(IServiceProvider services)
        {
            IHttpContextAccessor httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();
            ISession? session = httpContextAccessor.HttpContext?.Session;
            CoffeeShopDbContext context = services.GetRequiredService<CoffeeShopDbContext>();

            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();
            session?.SetString("CartId", cartId);

            return new ShoppingCartRepository(context, httpContextAccessor)
            {
                ShoppingCartId = cartId
            };
        }
        public void AddToCart(Product product)
        {
            var shoppingCartItem = dbContext.ShoppingCartItems
            .Include(s => s.Product)
            .FirstOrDefault(s => s.Product != null && s.Product.Id == product.Id && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Cty = 1,
                };
                dbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Cty++;
            }
            dbContext.SaveChanges();
            UpdateCartCount();
        }
        public void ClearCart()
        {
            var cartItems = dbContext.ShoppingCartItems.Where(s => s.ShoppingCartId ==
            ShoppingCartId);
            dbContext.ShoppingCartItems.RemoveRange(cartItems);
            dbContext.SaveChanges();
            UpdateCartCount();
        }
        public List<ShoppingCartItem> GetAllShoppingCartItems()
        {
            return ShoppingCartItems ??= dbContext.ShoppingCartItems.Where(s =>
            s.ShoppingCartId == ShoppingCartId).Include(p => p.Product).ToList();
        }
        public decimal GetShoppingCartTotal()
        {
            var totalCost = dbContext.ShoppingCartItems.Where(s => s.ShoppingCartId ==
            ShoppingCartId)
            .Where(s => s.Product != null)
            .Select(s => s.Product!.Price * s.Cty).Sum();

            return totalCost;
        }
        public int RemoveFromCart(Product product)
        {
            var shoppingCartItem = dbContext.ShoppingCartItems
            .Include(s => s.Product)
            .FirstOrDefault(s => s.Product != null && s.Product.Id == product.Id && s.ShoppingCartId == ShoppingCartId);

            var quantity = 0;
            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Cty > 1)
                {
                    shoppingCartItem.Cty--;
                    quantity = shoppingCartItem.Cty;
                }
                else
                {
                    dbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }
            dbContext.SaveChanges();
            UpdateCartCount();
            return quantity;
        }
        private void UpdateCartCount()
        {
            int cartCount = dbContext.ShoppingCartItems
                .Where(i => i.ShoppingCartId == ShoppingCartId)
                .Sum(i => i.Cty);

            _httpContextAccessor.HttpContext?.Session.SetInt32("CartCount", cartCount);
        }

    }
}
