using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MCApp.API.Data;
using MCApp.API.Dtos;
using MCApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController: Controller
    {
        private readonly IMicroCreditRepository _repo;
        private readonly IMapper _map;

        public UsersController(IMicroCreditRepository repo, IMapper map)
        {
            _repo = repo;
            _map = map;
        }        
       [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != id) return Unauthorized();
            var user = await _repo.GetUser(id);

            var userToReturn = _map.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }
       [HttpGet("name/{name}", Name="GetUserName")]
        public async Task<IActionResult> GetUser(string name)
        {
            var user = await _repo.GetUser(name.ToLower());
            if (user == null) return Unauthorized();

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != user.Id) return Unauthorized();

            var userToReturn = _map.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }
        // api/users/1 Put
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserForUpdateDto userForUpdateDto) {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (currentUserId != id) return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);

            if (userFromRepo == null) return NotFound($"Could not find user with id of {id}");

            _map.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll()) return NoContent();

            throw new Exception($"Updating user {id} failed on save");

        }
     }
}