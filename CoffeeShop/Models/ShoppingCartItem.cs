namespace CoffeeShop.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public Product? Product { get; set; }
        public int Cty { get; set; }
        public string? ShoppingCartId { get; set; }
    }
}
