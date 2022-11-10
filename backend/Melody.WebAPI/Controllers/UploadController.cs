using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Melody.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private const string soundExtension = ".bmp";
        private const string folderName = "Sounds";
        [HttpPost]
        public IActionResult UploadFile(IFormFile uploadedSoundFile)
        {
            var extension = Path.GetExtension(uploadedSoundFile.FileName);
            /*if (extension != soundExtension)
            {
                return BadRequest();
            }*/

            var currentDirectory = Directory.GetCurrentDirectory();
            var pathToSave = Path.Combine(currentDirectory, folderName);
            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            string fileName = Path.GetFileName(uploadedSoundFile.FileName);
            var guidFileName = Guid.NewGuid().ToString() + soundExtension;
            var guidSubFolders = string.Empty;
            for(int i = 0; i < 6; i = i + 2)
            {
                var guidSubstr = guidFileName.Substring(i, 2);
                guidSubFolders = Path.Combine(guidSubFolders, guidSubstr);
                var currentPath = Path.Combine(pathToSave, guidSubFolders);
                if (!Directory.Exists(currentPath))
                {
                    Directory.CreateDirectory(currentPath);
                }
            }
            var fullPath = Path.Combine(pathToSave, guidSubFolders, guidFileName);
            
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                uploadedSoundFile.CopyTo(stream);
            }
            return Ok(Path.Combine(folderName, guidSubFolders, guidFileName));
        }
    }
}
