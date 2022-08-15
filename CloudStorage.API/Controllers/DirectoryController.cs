using CloudStorage.Core.Dtos;
using CloudStorage.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DirectoryController : BaseController
{
    private readonly IDirectoryService _directoryService;

    public DirectoryController(IDirectoryService directoryService)
        => _directoryService = directoryService;

    /// <summary>
    /// Get all items in main directory
    /// </summary>
    /// <returns>List of ItemDto</returns>

    [HttpGet]
    public async Task<ActionResult<List<ItemDto>>> GetItemsInMain()
        => await _directoryService.GetAllInCurrent(UserId, null);

    /// <summary>
    /// Get all items in current directory
    /// </summary>
    /// <param name="currentFolderId">Current directory id</param>
    /// <returns>List of ItemDto</returns>

    [HttpGet("folders/{id}")]
    public async Task<ActionResult<List<ItemDto>>> GetItemsInFolder(Guid currentFolderId)
        => await _directoryService.GetAllInCurrent(UserId, currentFolderId);
}
