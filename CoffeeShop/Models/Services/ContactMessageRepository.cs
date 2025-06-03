using CoffeeShop.Models;
using CoffeeShop.Models.Interfaces;
using CoffeeShop.Data;

namespace CoffeeShop.Models.Services
{
    public class ContactMessageRepository:IContactMessageRepository
    {
        private readonly CoffeeShopDbContext _context;
        public ContactMessageRepository(CoffeeShopDbContext context)
        {
            _context = context;
        }
        public void AddContactMessage(ContactMessage message)
        {
            _context.ContactMessages.Add(message);
            _context.SaveChanges();
        }
        public IEnumerable<ContactMessage> GetAllContactMessages()
        {
            return _context.ContactMessages.OrderByDescending(m => m.SentDate).ToList();
        }
    }
}
