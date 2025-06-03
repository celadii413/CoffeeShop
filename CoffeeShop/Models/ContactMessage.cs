using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Display(Name = "Your Name")] 
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Your Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your message.")]
        [StringLength(2000, ErrorMessage = "Message cannot exceed 2000 characters.")] 
        public string Message { get; set; }

        [Display(Name = "Sent Date")]
        public DateTime SentDate { get; set; } = DateTime.Now;
    }
}
