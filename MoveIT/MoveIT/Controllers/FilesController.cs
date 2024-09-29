using Microsoft.AspNetCore.Mvc;
using MoveIT.Services.Contracts;

namespace MoveIT.Controllers
{
    public class FilesController : Controller
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(string path)
        {
            await _fileService.Upload(path);

            return View();
        }
    }
}
