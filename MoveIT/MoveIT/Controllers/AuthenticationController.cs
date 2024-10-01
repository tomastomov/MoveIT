using Microsoft.AspNetCore.Mvc;
using MoveIT.Gateways.Contracts;
using MoveIT.Models.Authentication;
using MoveIT.Services.Contracts;
using static MoveIT.Common.Constants;

namespace MoveIT.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IHttpContextAccessor contextAccessor, IAuthenticationService authenticationService)
        {
            _contextAccessor = contextAccessor;
            _authenticationService = authenticationService;
        }

        public async Task<IActionResult> Login()
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
            _contextAccessor.HttpContext.Session.Remove(JWT);

            return RedirectToAction(INDEX_ACTION, HOME_CONTROLLER);
        }
    }
}
