using Microsoft.AspNetCore.Mvc;

namespace MoveIT.Controllers
{
    public class FilesController : Controller
    {
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
    }
}
