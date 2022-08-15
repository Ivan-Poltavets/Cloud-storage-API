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
        private readonly IFolderService _folderService;

        public FolderController(IDirectoryService directoryService, IFolderService folderService)
        {
            _directoryService = directoryService;
            _folderService = folderService;
        }

        /// <summary>
        /// Get all items in main directory
        /// </summary>
        /// <returns>List of ItemDto</returns>

        [HttpGet]
        public async Task<ActionResult<List<ItemDto>>> GetFiles()
            => await _directoryService.GetAllInCurrent(UserId, null);

        /// <summary>
        /// Get all items in current directory
        /// </summary>
        /// <param name="currentFolderId">Current directory id</param>
        /// <returns>List of ItemDto</returns>
        /// 

        [HttpGet("folders/")]
        public async Task<ActionResult<List<ItemDto>>> GetFolders(Guid currentFolderId)
            => await _directoryService.GetAllInCurrent(UserId, currentFolderId);

        /// <summary>
        /// Add folder and create folder info
        /// </summary>
        /// <param name="folderDto"></param>
        /// <returns>Status code 201</returns>

        [HttpPost]
        [Route("add-folder")]
        public async Task<IActionResult> AddFolder(FolderDto folderDto, Guid? currentFolderId)
        {
            await _folderService.AddFolder(folderDto, UserId, currentFolderId);
            return CreatedAtAction(nameof(AddFolder), folderDto);
        }

        /// <summary>
        /// Remove folder and remove folder info
        /// </summary>
        /// <param name="id">Directory id</param>
        /// <returns>Status code</returns>

        [HttpDelete]
        [Route("remove-folder")]
        public async Task<IActionResult> RemoveFolder(Guid id)
        {
            await _folderService.RemoveFolder(id);
            return NoContent();
        }
    }
}
