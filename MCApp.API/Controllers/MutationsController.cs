using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MCApp.API.BackgroundServices;
using MCApp.API.Data;
using MCApp.API.Dtos;
using MCApp.API.Helpers;
using MCApp.API.Models;
using MCApp.API.ScheduledServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/Accounts/{accountId}/[controller]")]

    public class MutationsController: Controller
    {
        private readonly IMicroCreditRepository _repo;
        private readonly IMapper _map;
        private  ScheduleTable schedulePollerProcess;

        public MutationsController(IMicroCreditRepository repo, IMapper map)
        {
            _repo = repo;
            _map = map;
            schedulePollerProcess = ScheduleTable.GetScheduleForProcess<PollerProcess>();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMutation(int userId, int accountId, [FromBody]MutationForCreationDto mutationForCreationDto) {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId) return Unauthorized();
        
            if (!ModelState.IsValid) return BadRequest(ModelState);

            mutationForCreationDto.UserId = userId;
            mutationForCreationDto.AccountId = accountId;

            var account =  await _repo.GetAccount(accountId );       
            if ( account == null  || account.UserId != userId) return Unauthorized();
            var user = await _repo.GetUser(userId);

            // eens kijken of de mapper zo slim is dat ie de sender informatie ook invult in messageToReturn
            // zelfs bij de sender info die nu ietsanders heet.
            var mutation = _map.Map<Mutation>(mutationForCreationDto);

            var lastMutation = await _repo.GetMutation(account.LastMutationCreated, userId, accountId);
            if ( lastMutation == null) {
                mutation.Balance = mutation.Amount;
                mutation.PrevId = -account.Id;
            } else {
                mutation.PrevId = lastMutation.Id;
                mutation.Balance = lastMutation.Balance + mutation.Amount;
                var diffdays = (mutation.InterestDate - lastMutation.InterestDate).TotalDays;
                int nrdays = (int) diffdays;
                if ( lastMutation.InterestDate.AddDays(diffdays).DayOfYear != mutation.InterestDate.DayOfYear ) {
                    nrdays += 1;
                }
                // zoek eerst nog de gemiddelde rente op dus alle percentages af totdat 
                var interest = (nrdays / 365) * account.Percentage;
                account.CalculatedInterest += interest;
            }
            if (mutation.Percentage != 0.0) {
                account.Percentage = mutation.Percentage;
            } else {
                mutation.Percentage = account.Percentage;
            }
            account.Balance = mutation.Balance;
            account.LastMutationCreated = mutation.Created;
            account.LastMutation = mutation;

            _repo.Add(mutation);
            if (await _repo.SaveAll()) {
                var mutationToReturn = _map.Map<MutationForDetailedDto>(mutation);
                ScheduleTable.StartProcess(schedulePollerProcess, 5000);
                return CreatedAtRoute("GetMutation", new { id = mutation.Id }, mutationToReturn);
            }
            throw new Exception("Creating the mutation failed on save");
        }

       [HttpGet("{id}", Name="GetMutation")]
        public async Task<IActionResult> GetMutation(int id, int userId, int accountId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId) return Unauthorized();

            var account =  await _repo.GetAccount(accountId );       
            if ( account == null  || account.UserId != userId) return Unauthorized();

            var mutation = await _repo.GetMutation(id);
            if ( mutation == null  || mutation.UserId != userId || mutation.AccountId != accountId) return Unauthorized();

            var user = await _repo.GetUser(userId);

            var mutationToReturn = _map.Map<MutationForDetailedDto>(mutation);
            return Ok(mutationToReturn);
        }
        [HttpGet]
        public async Task<IActionResult> GetMutationsForUserAccount(int userId, int accountId, MutationParams mutationParams) {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != userId) return Unauthorized();

            var account =  await _repo.GetAccount(accountId );       
            if ( account == null  || account.UserId != userId) return Unauthorized();

            var user = await _repo.GetUser(userId);

            mutationParams.UserId = userId;
            mutationParams.AccountId = accountId;

            var pgMutationsFromRepo = await _repo.GetMutationsForUserAccount(mutationParams);
            var accDisply = _map.Map<AccountForDetailedDto>(account);
            var mutations = _map.Map<ICollection<MutationForListDto>>(pgMutationsFromRepo);
            Response.AddPagination(pgMutationsFromRepo.CurrentPage, pgMutationsFromRepo.PageSize,
                pgMutationsFromRepo.TotalCount, pgMutationsFromRepo.TotalPages);
            return Ok(new MutationForPageDto { Account= accDisply, Mutations = mutations });
        }
        
    }
}