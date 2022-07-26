using CloudStorage.Core.Dtos;
using CloudStorage.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CloudStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        internal Guid UserId => User.Identity!.IsAuthenticated
            ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value) : Guid.Empty;

        private readonly IManageService _manageService;
        private readonly IDirectoryService _directoryService;

        public HomeController(IManageService manageService, IDirectoryService directoryService)
        {
            _manageService = manageService;
            _directoryService = directoryService;
        }


        [Authorize]
        [HttpGet("{currentDirectory}")]
        public ActionResult<List<FileDto>> GetFiles(string currentDirectory = "")
        {
            return _directoryService.GetAllInCurrent(UserId, currentDirectory);
        }

        [Authorize]
        [HttpPost]
        [Route("add-files")]
        public IActionResult AddFiles(List<IFormFile> files, string currentDirectory)
        {
            _manageService.AddFiles(files, UserId, currentDirectory);
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        [Route("remove-files")]
        public IActionResult RemoveFiles(List<string> names, string currentDirectory)
        {
            _manageService.RemoveFiles(names, UserId);
            return NoContent();
        }

        [Authorize]
        [HttpPost]
        [Route("add-folder")]
        public IActionResult AddFolder(FolderDto folderDto)
        {
            _manageService.AddFolder(folderDto, UserId);
            return CreatedAtAction(nameof(AddFolder), folderDto);
        }

        [Authorize]
        [HttpDelete]
        [Route("remove-folder")]
        public IActionResult RemoveFolder(FolderDto folderDto)
        {
            _manageService.RemoveFolder(folderDto, UserId);
            return NoContent();
        }
    }
}
