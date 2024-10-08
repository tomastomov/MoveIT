﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MoveIT.Gateways.Contracts.Models;
using MoveIT.Models.Files;
using MoveIT.Services.Contracts;
using static MoveIT.Common.Constants;

namespace MoveIT.Controllers
{
    public class FilesController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IOptions<MoveITOptions> _options;

        public FilesController(IFileService fileService, IOptions<MoveITOptions> options)
        {
            _fileService = fileService;
            _options = options;
        }

        [HttpGet]
        [Authorize(Policy = HAS_TOKEN_POLICY)]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = HAS_TOKEN_POLICY)]
        public async Task<IActionResult> Upload(UploadFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _fileService.Upload(async () =>
            {
                using var memoryStream = new MemoryStream();
                await model.File.CopyToAsync(memoryStream);
                return (memoryStream.ToArray(), model.File.FileName);
            }, _options.Value.BASE_FOLDER_ID);

            if (result.ErrorMessage is not null)
            {
                TempData[ERROR_MESSAGE] = result.ErrorMessage;
            } else
            {
                TempData[SUCCESS_MESSAGE] = FILE_UPLOADED_SUCCESSFULLY_MESSAGE;
            }
            
            return View();
        }
    }
}
