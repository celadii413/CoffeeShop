using CoffeeShop.Models;
using CoffeeShop.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoffeeShop.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactMessageRepository _contactMessageRepository;
        public ContactController(IContactMessageRepository contactMessageRepository)
        {
            _contactMessageRepository = contactMessageRepository;
        }
        [HttpGet]
        public IActionResult AddContact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddContact(ContactMessage model)
        {

            if (ModelState.IsValid)
            {
                _contactMessageRepository.AddContactMessage(model);
                TempData["Message"] = "Your message has been sent successfully!";
                return RedirectToAction("ContactConfirmation");
            }

            return View(model);
        }

        public IActionResult ContactConfirmation()
        {
            return View();
        }

    }
}
