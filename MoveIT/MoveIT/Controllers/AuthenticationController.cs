using Microsoft.AspNetCore.Mvc;
using MoveIT.Common.Contracts;
using MoveIT.Models.Authentication;
using MoveIT.Services.Contracts;
using static MoveIT.Common.Constants;

namespace MoveIT.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenManager _tokenManager;

        public AuthenticationController(IAuthenticationService authenticationService, ITokenManager tokenManager)
        {
            _authenticationService = authenticationService;
            _tokenManager = tokenManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authenticationService.Authenticate();

            if (result.ErrorMessage is not null)
            {
                TempData[ERROR_MESSAGE] = result.ErrorMessage;
                return View(model);
            }

            return RedirectToAction(UPLOAD_ACTION, FILES_CONTROLLER);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _tokenManager.DeleteToken();

            return RedirectToAction(INDEX_ACTION, HOME_CONTROLLER);
        }
    }
}
