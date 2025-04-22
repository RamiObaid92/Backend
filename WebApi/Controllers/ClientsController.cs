using Business.Services;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions.Attributes;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [UseAdminApiKey]
    [ApiController]
    public class ClientsController(IClientService clientService) : ControllerBase
    {
        private readonly IClientService _clientService = clientService;

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromForm] AddClientForm addClientForm)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var client = await _clientService.CreateClientAsync(addClientForm);

            if (client is null) 
                return BadRequest();

            return Ok(client);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _clientService.GetAllClientsAsync();

            if (clients is null) 
                return NotFound();

            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(Guid id)
        {
            var client = await _clientService.GetClientByIdAsync(id);

            if (client is null) 
                return NotFound();

            return Ok(client);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClient([FromForm] EditClientForm editClientForm)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var client = await _clientService.UpdateClientAsync(editClientForm);

            if (client is null) 
                return NotFound();

            return Ok(client);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            var result = await _clientService.DeleteClientAsync(id);

            if (!result) 
                return NotFound();

            return Ok();
        }
    }
}
