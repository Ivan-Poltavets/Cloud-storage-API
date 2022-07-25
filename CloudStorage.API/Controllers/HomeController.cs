using CloudStorage.Core.Dtos;
using CloudStorage.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IManageService _manageService;
        private readonly IDirectoryService _directoryService;

        public HomeController(IManageService manageService, IDirectoryService directoryService)
        {
            _manageService = manageService;
            _directoryService = directoryService;
        }


        [Authorize]
        [HttpGet("{currentDirectory}")]
        public ActionResult<List<FileDto>> GetFiles(string currentDirectory)
        {
            return _directoryService.GetAllInCurrent(Guid.NewGuid(), currentDirectory);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddFiles(List<IFormFile> files, string currentDirectory)
        {
            _manageService.AddFiles(files, Guid.NewGuid(), currentDirectory);
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public IActionResult RemoveFiles(List<Guid> ids)
        {
            _manageService.RemoveFiles(ids);
            return NoContent();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddFolder(FolderDto folderDto)
        {
            _manageService.AddFolder(folderDto);
            return CreatedAtAction(nameof(AddFolder), folderDto);
        }

        [Authorize]
        [HttpDelete]
        public IActionResult RemoveFolder(FolderDto folderDto)
        {
            _manageService.RemoveFolder(folderDto);
            return NoContent();
        }
    }
}
