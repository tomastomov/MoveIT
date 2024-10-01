using System.ComponentModel.DataAnnotations;
using static MoveIT.Common.Constants;

namespace MoveIT.Models.Files
{
    public class UploadFileViewModel
    {
        [Required(ErrorMessage = FILE_REQUIRED_ERROR_MESSAGE)]
        public IFormFile File { get; set; }
    }
}
