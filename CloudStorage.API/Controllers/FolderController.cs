using CloudStorage.Core.Dtos;
using CloudStorage.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : BaseController
    {
        private readonly IDirectoryService _directoryService;
        private readonly IManageService _manageService;

        public FolderController(IDirectoryService directoryService, IManageService manageService)
        {
            _directoryService = directoryService;
            _manageService = manageService;
        }

        /// <summary>
        /// Get all items in current directory
        /// </summary>
        /// <param name="currentDirectory">Current directory</param>
        /// <returns>List of FileDtos</returns>

        [HttpGet]
        public async Task<ActionResult<List<FileDto>>> GetFiles()
        {
            return await _directoryService.GetAllInCurrent(UserId, null);
        }

        [HttpGet("folders/{id}")]
        public async Task<ActionResult<List<FileDto>>> GetFolder(Guid currentFolderId)
        {
            return await _directoryService.GetAllInCurrent(UserId, currentFolderId);
        }

        /// <summary>
        /// Add folder and create folder info
        /// </summary>
        /// <param name="folderDto"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("add-folder")]
        public async Task<IActionResult> AddFolder(FolderDto folderDto, Guid? currentFolderId)
        {
            await _manageService.AddFolder(folderDto, UserId, currentFolderId);
            return CreatedAtAction(nameof(AddFolder), folderDto);
        }

        /// <summary>
        /// Remove folder and remove folder info
        /// </summary>
        /// <param name="folderDto">Folder name and path</param>
        /// <returns>Status code</returns>

        [HttpDelete]
        [Route("remove-folder")]
        public async Task<IActionResult> RemoveFolder(Guid id)
        {
            await _manageService.RemoveFolder(id);
            return NoContent();
        }
    }
}
