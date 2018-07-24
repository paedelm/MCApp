using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MCApp.API.Data;
using MCApp.API.Dtos;
using MCApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/Accounts/{accountId}/[controller]")]
    public class InterestsController: Controller
    {

        private readonly IMicroCreditRepository _repo;
        private readonly IMapper _map;

        public InterestsController(IMicroCreditRepository repo, IMapper map)
        {
            _repo = repo;
            _map = map;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInterest(int userId, int accountId, [FromBody]InterestForCreationDto interestForCreationDto) {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId) return Unauthorized();
        
            if (!ModelState.IsValid) return BadRequest(ModelState);

            interestForCreationDto.UserId = userId;
            interestForCreationDto.AccountId = accountId;

            var account =  await _repo.GetAccount(accountId );       
            if ( account == null  || account.UserId != userId) return Unauthorized();
            var user = await _repo.GetUser(userId);

            // eens kijken of de mapper zo slim is dat ie de sender informatie ook invult in messageToReturn
            // zelfs bij de sender info die nu ietsanders heet.
            var interest = _map.Map<Interest>(interestForCreationDto);

            account.Percentage = interest.Percentage;

            _repo.Add(interest);
            if (await _repo.SaveAll()) {
                var interestToReturn = _map.Map<InterestForDetailedDto>(interest);
                return CreatedAtRoute("GetInterest", new { id = interest.Id }, interestToReturn);
            }
            throw new Exception("Creating the mutation failed on save");
        }

       [HttpGet("{id}", Name="GetInterest")]
        public async Task<IActionResult> GetInterest(int id, int userId, int accountId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId) return Unauthorized();

            var account =  await _repo.GetAccount(accountId );       
            if ( account == null  || account.UserId != userId) return Unauthorized();

            var interest = await _repo.GetInterest(id);
            if ( interest == null  || interest.UserId != userId || interest.AccountId != accountId) return Unauthorized();

            var user = await _repo.GetUser(userId);

            var interestToReturn = _map.Map<InterestForDetailedDto>(interest);
            return Ok(interestToReturn);
        }

    }
        
}