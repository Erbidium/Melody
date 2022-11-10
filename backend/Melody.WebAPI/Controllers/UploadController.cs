using Melody.Core.Entities;
using Melody.Infrastructure.Data.Records;
using Melody.Infrastructure.Data.Repositories;
using Melody.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;

namespace Melody.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ISongRepository _songRepository;
        private readonly TokenService _tokenService;
        private readonly UserManager<UserIdentity> _userManager;
        public UploadController(ISongRepository songRepository, TokenService tokenService, UserManager<UserIdentity> userManager)
        {
            _songRepository = songRepository;
            _tokenService = tokenService;
            _userManager = userManager;
        }
        private const string soundExtension = ".bmp";
        private const string folderName = "Sounds";
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile uploadedSoundFile)
        {
            // TODO: check total user uploads size by sql query by filtering isDeleted
            var extension = Path.GetExtension(uploadedSoundFile.FileName);
            if (extension != soundExtension)
            {
                return BadRequest();
            }

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
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var currentUserFromToken = _tokenService.GetCurrentUser(identity);
            var user = await _userManager.FindByEmailAsync(currentUserFromToken.Email);
            // TODO: write information to db about user uploader, upload date, upload size with IsDeleted property
            return Ok(Path.Combine(folderName, guidSubFolders, guidFileName));
        }

        // TODO: add ability to delete files
    }
}
