using System.ComponentModel.DataAnnotations;

namespace MoveIT.Models.Authentication
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
