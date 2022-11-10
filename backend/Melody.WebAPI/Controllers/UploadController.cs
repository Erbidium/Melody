using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Melody.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {

        [HttpPost]
        public IActionResult UploadFile(IFormFile uploadedSoundFile)
        {
            var folderName = Path.Combine("Sounds");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            string fileName = Path.GetFileName(uploadedSoundFile.FileName);
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                uploadedSoundFile.CopyTo(stream);
            }
            return Ok(new { dbPath });
        }
    }
}
