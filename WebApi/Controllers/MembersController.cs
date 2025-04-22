using Business.Services;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Documentation;
using WebApi.Extensions.Attributes;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [UseAdminApiKey]
    [ApiController]
    public class MembersController(IMemberService memberService) : ControllerBase
    {
        private readonly IMemberService _memberService = memberService;

        [HttpPost]
        [SwaggerOperation(Summary = "Add a new member")]
        [SwaggerRequestExample(typeof(AddMemberForm), typeof(AddMemberDataExample))]
        [SwaggerResponseExample(400, typeof(UserValidationErrorExample))]
        [ProducesResponseType(typeof(MemberModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMember([FromForm] AddMemberForm addMemberForm)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var member = await _memberService.CreateMemberAsync(addMemberForm);

            if (member is null) 
                return BadRequest();

            return Ok(member);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMembers()
        {
            var members = await _memberService.GetAllMembersAsync();

            if (members is null) 
                return NotFound();

            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberById(Guid id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);

            if (member is null) 
                return NotFound();

            return Ok(member);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Edit a member")]
        [SwaggerRequestExample(typeof(EditMemberForm), typeof(EditMemberDataExample))]
        [SwaggerResponseExample(400, typeof(UserValidationErrorExample))]
        [ProducesResponseType(typeof(MemberModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMember([FromForm] EditMemberForm editMemberForm)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var member = await _memberService.UpdateMemberAsync(editMemberForm);

            if (member is null) 
                return NotFound();

            return Ok(member);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(Guid id)
        {
            var result = await _memberService.DeleteMemberAsync(id);

            if (!result) 
                return NotFound();

            return Ok();
        }
    }
}
