using CloudStorage.Core.Dtos;
using CloudStorage.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : BaseController
    {
        private readonly IFolderService _folderService;

        public FolderController(IFolderService folderService)
            => _folderService = folderService;

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
