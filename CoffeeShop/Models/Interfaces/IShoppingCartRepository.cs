using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Models.Interfaces
{
    public interface IShoppingCartRepository
    {
        void AddToCart(Product product);
        int RemoveFromCart(Product product);
        List<ShoppingCartItem> GetAllShoppingCartItems();
        public void ClearCart();
        decimal GetShoppingCartTotal();
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
