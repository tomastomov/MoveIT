using Microsoft.AspNetCore.Mvc;
using MoveIT.Gateways.Contracts;
using MoveIT.Models.Authentication;
using static MoveIT.Common.Constants;

namespace MoveIT.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IMoveITGateway _gateway;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthenticationController(IMoveITGateway gateway, IHttpContextAccessor contextAccessor)
        {
            _gateway = gateway;
            _contextAccessor = contextAccessor;
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

            var token = await _gateway.Login(model.Username);

            if (token is null)
            {
                return View(model);
            }

            _contextAccessor.HttpContext.Session.SetString(JWT, token);

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
