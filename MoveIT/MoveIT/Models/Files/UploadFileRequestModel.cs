using System.ComponentModel.DataAnnotations;

namespace MoveIT.Models.Files
{
    public class UploadFileRequestModel
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
