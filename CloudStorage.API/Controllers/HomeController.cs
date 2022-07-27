using CloudStorage.Core.Dtos;
using CloudStorage.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CloudStorage.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        internal string UserId => User.Identity!.IsAuthenticated
            ? User.FindFirst(ClaimTypes.NameIdentifier)!.Value : string.Empty;

        private readonly IManageService _manageService;
        private readonly IDirectoryService _directoryService;

        public HomeController(IManageService manageService, IDirectoryService directoryService)
        {
            _manageService = manageService;
            _directoryService = directoryService;
        }

        /// <summary>
        /// Get all items in current directory
        /// </summary>
        /// <param name="currentDirectory">Current directory</param>
        /// <returns>List of FileDtos</returns>

        [HttpGet("{currentDirectory}")]
        public ActionResult<List<FileDto>> GetFiles(string currentDirectory)
        {
            return _directoryService.GetAllInCurrent(UserId, currentDirectory);
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
        public async Task<IActionResult> AddFiles(List<IFormFile> files, string currentDirectory)
        {
            await _manageService.AddFiles(files, UserId, currentDirectory);
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
        public async Task<IActionResult> RemoveFiles(List<string> names, string currentDirectory)
        {
            await _manageService.RemoveFiles(names, UserId, currentDirectory);
            return NoContent();
        }

        /// <summary>
        /// Add folder and create folder info
        /// </summary>
        /// <param name="folderDto"></param>
        /// <returns></returns>
      
        [HttpPost]
        [Route("add-folder")]
        public async Task<IActionResult> AddFolder(FolderDto folderDto)
        {
            await _manageService.AddFolder(folderDto, UserId);
            return CreatedAtAction(nameof(AddFolder), folderDto);
        }

        /// <summary>
        /// Remove folder and remove folder info
        /// </summary>
        /// <param name="folderDto">Folder name and path</param>
        /// <returns>Status code</returns>

        [HttpDelete]
        [Route("remove-folder")]
        public async Task<IActionResult> RemoveFolder(FolderDto folderDto)
        {
            await _manageService.RemoveFolder(folderDto, UserId);
            return NoContent();
        }
    }
}
