using Business.Services;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions.Attributes;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [RequireKey("AdminKey")]
    [ApiController]
    public class MembersController(IMemberService memberService) : ControllerBase
    {
        private readonly IMemberService _memberService = memberService;

        [HttpPost]
        public async Task<IActionResult> CreateMember(AddMemberForm addMemberForm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var member = await _memberService.CreateMemberAsync(addMemberForm);
            if (member is null) return BadRequest();
            return Ok(member);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMembers()
        {
            var members = await _memberService.GetAllMembersAsync();
            if (members is null) return NotFound();
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberById(Guid id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member is null) return NotFound();
            return Ok(member);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMember(EditMemberForm editMemberForm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var member = await _memberService.UpdateMemberAsync(editMemberForm);
            if (member is null) return NotFound();
            return Ok(member);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(Guid id)
        {
            var result = await _memberService.DeleteMemberAsync(id);
            if (!result) return NotFound();
            return Ok();
        }
    }
}
