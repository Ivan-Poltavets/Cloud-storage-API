using CloudStorage.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileController : BaseController
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
        => _fileService = fileService;

    /// <summary>
    /// Upload files to Blob Storage, save info about files, limit size of files in account
    /// </summary>
    /// <param name="files">Uploading files</param>
    /// <param name="currentFolderId">Current directory id</param>
    /// <returns>Status code 201</returns>

    [HttpPost]
    public async Task<IActionResult> AddFiles(List<IFormFile> files, string? currentFolderId)
    {
        await _fileService.AddFilesAsync(files, UserId, currentFolderId);
        return CreatedAtAction(nameof(AddFiles), files);
    }

    /// <summary>
    /// Remove all files by id
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>

    [HttpDelete]
    public async Task<IActionResult> RemoveFiles(List<string> ids)
    {
        await _fileService.RemoveFilesAsync(ids, UserId);
        return NoContent();
    }
}
