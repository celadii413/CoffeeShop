using CoffeeShop.Models;
namespace CoffeeShop.Models.Interfaces
{
    public interface IContactMessageRepository
    {
        void AddContactMessage(ContactMessage message);
        IEnumerable<ContactMessage> GetAllContactMessages();
    }
}
