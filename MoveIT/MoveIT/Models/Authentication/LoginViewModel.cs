using System.ComponentModel.DataAnnotations;
using static MoveIT.Common.Constants;

namespace MoveIT.Models.Authentication
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = USERNAME_REQUIRED_ERROR_MESSAGE)]
        public string Username { get; set; }

        [Required(ErrorMessage = PASSWORD_REQUIRED_ERROR_MESSAGE)]
        public string Password { get; set; }
    }
}
