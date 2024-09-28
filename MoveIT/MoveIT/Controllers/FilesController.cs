using Microsoft.AspNetCore.Mvc;

namespace MoveIT.Controllers
{
    [Route("files")]
    public class FilesController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            return View();
        }
    }
}
