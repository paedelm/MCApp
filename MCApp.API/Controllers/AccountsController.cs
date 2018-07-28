using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MCApp.API.Data;
using MCApp.API.Dtos;
using MCApp.API.Helpers;
using MCApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    public class AccountsController: Controller
    {
        private readonly IMicroCreditRepository _repo;
        private readonly IMapper _map;

        public AccountsController(IMicroCreditRepository repo, IMapper map)
        {
            _repo = repo;
            _map = map;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(int userId, [FromBody]AccountForCreationDto accountForCreationDto) {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId) return Unauthorized();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            accountForCreationDto.UserId = userId;

        
            if (await _repo.GetAccount(userId, accountForCreationDto.Accountname ) != null) return BadRequest("Account already exists");
        
            var user = await _repo.GetUser(accountForCreationDto.UserId);
            // eens kijken of de mapper zo slim is dat ie de sender informatie ook invult in messageToReturn
            // zelfs bij de sender info die nu ietsanders heet.
            var account = _map.Map<Account>(accountForCreationDto);
            account.Balance = 0;
            _repo.Add(account);
            if (await _repo.SaveAll()) {
                var accountToReturn = _map.Map<AccountForDetailedDto>(account);
                return CreatedAtRoute("GetAccount", new { id = account.Id }, accountToReturn);
            }
            throw new Exception("Creating the account failed on save");
        }
       [HttpGet("{id}", Name="GetAccount")]
        public async Task<IActionResult> GetAccount(int id, int userId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId) return Unauthorized();

            var user = await _repo.GetUser(userId);

            var account = await _repo.GetAccount(id);
            if (account == null || account.UserId != userId) return Unauthorized();

            var lastMutation = await _repo.GetMutation(account.LastMutationCreated, userId, id);
            if (lastMutation != null) {
                account.LastMutation = lastMutation;
                _map.Map<MutationForDetailedDto>(lastMutation);
            }
            var accountToReturn = _map.Map<AccountForDetailedDto>(account);
            return Ok(accountToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts(int userId) {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId) return Unauthorized();

            var user = await _repo.GetUser(userId);
            
            var userToReturn = _map.Map<UserWithAccountsDto>(user);
            return Ok(userToReturn);
        }
    }
}