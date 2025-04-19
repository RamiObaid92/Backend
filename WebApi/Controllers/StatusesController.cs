using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class StatusesController(IStatusService statusService) : ControllerBase
    {
        private readonly IStatusService _statusService = statusService;

        [HttpGet]
        public async Task<IActionResult> GetAllStatuses()
        {
            var statuses = await _statusService.GetAllStatusesAsync();
            if (statuses is null) return NotFound();
            return Ok(statuses);
        }
    }
}
