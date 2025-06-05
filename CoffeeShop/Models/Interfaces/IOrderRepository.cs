namespace CoffeeShop.Models.Interfaces
{
    public interface IOrderRepository
    {
        void PlaceOrder(Order order);
        Task<IEnumerable<Order>> GetOrdersForUserAsync(string userId);
        Order? GetOrderById(int orderId);
    }
}
