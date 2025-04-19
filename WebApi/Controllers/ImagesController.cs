using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions.Attributes;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [RequireKey("AdminKey, UserKey")]
    [ApiController]
    public class ImagesController(IImageCatalogService imageCatalogService) : ControllerBase
    {
        private readonly IImageCatalogService _catalog = imageCatalogService;

        [HttpGet("AvatarUrls")]
        public IActionResult GetAvatars()
            => Ok(_catalog.GetAvatars());

        [HttpGet("ProjectUrls")]
        public IActionResult GetProjectPics()
            => Ok(_catalog.GetProjectPics());
    }
}
