using CoffeeShop.Data;
using CoffeeShop.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShop.Models.Services
{
    public class OrderRepository:IOrderRepository
    {
        private CoffeeShopDbContext dbContext;
        private IShoppingCartRepository shoppingCartRepository;
        public OrderRepository(CoffeeShopDbContext dbContext, IShoppingCartRepository shoppingCartRepository)
        {
            this.dbContext = dbContext;
            this.shoppingCartRepository = shoppingCartRepository;
        }
        public void PlaceOrder(Order order)
        {
            var shoppingCartItems = shoppingCartRepository.GetAllShoppingCartItems();
            order.OrderDetails = new List<OrderDetail>();
            foreach (var item in shoppingCartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Quantity = item.Cty,
                    ProductId = item.Product.Id,
                    Price = item.Product.Price
                };
                order.OrderDetails.Add(orderDetail);
            }
            order.OrderPlaced = DateTime.Now;
            order.OrderTotal = shoppingCartRepository.GetShoppingCartTotal();   
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();
            shoppingCartRepository.ClearCart();
        }

        public async Task<IEnumerable<Order>> GetOrdersForUserAsync(string userId)
        {
            return await dbContext.Orders
                                    .Where(o => o.UserId == userId)
                                    .Include(o => o.OrderDetails!) 
                                        .ThenInclude(od => od.Product!) 
                                    .OrderByDescending(o => o.OrderPlaced)
                                    .ToListAsync();
        }

        public Order? GetOrderById(int orderId)
        {
            return dbContext.Orders
                            .Include(o => o.OrderDetails!) 
                            .ThenInclude(od => od.Product!) 
                            .FirstOrDefault(o => o.Id == orderId);
        }
    }
}
