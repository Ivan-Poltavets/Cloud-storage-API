using CloudStorage.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : BaseController
    {
        private readonly IManageService _manageService;

        public FileController(IManageService manageService)
        {
            _manageService = manageService;
        }

        /// <summary>
        /// Upload files to Blob Storage,
        /// save info about files, 
        /// limit size of files in account
        /// </summary>
        /// <param name="files">Uploading files</param>
        /// <param name="currentDirectory">Current directory</param>
        /// <returns>Status code</returns>

        [HttpPost]
        [Route("add-files")]
        public async Task<IActionResult> AddFiles(List<IFormFile> files, Guid? currentFolderId)
        {
            await _manageService.AddFiles(files, UserId, currentFolderId);
            return Ok();
        }

        /// <summary>
        /// Remove files from Blob Storage, also remove info, storage info 
        /// </summary>
        /// <param name="names">Names of files</param>
        /// <param name="currentDirectory">Current directory</param>
        /// <returns>Status code</returns>

        [HttpDelete]
        [Route("remove-files")]
        public async Task<IActionResult> RemoveFiles(List<string> names, Guid? currentFolderId)
        {
            await _manageService.RemoveFiles(names, UserId, currentFolderId);
            return NoContent();
        }
    }
}
