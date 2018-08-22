using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCApp.API.Helpers;
using MCApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MCApp.API.Data
{
    public class MicroCreditRepository : IMicroCreditRepository
    {
        private readonly DataContext _context;
        public MicroCreditRepository(DataContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Account> GetAccount(int id)
        {
            return await _context.Accounts
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Account> GetAccount(int userId, string name)
        {
            return await _context.Accounts
                .Where(a => a.UserId == userId)
                .FirstOrDefaultAsync(a => a.Accountname == name);
        }

        public async Task<PagedList<Mutation>> GetAccountMutationsForUser(int userId, int accountId, MutationParams mp)
        {
            var mutations = _context.Mutations
                .Where(m => m.UserId == userId && m.AccountId == accountId)
                .OrderByDescending(m => m.Created);
            return await PagedList<Mutation>.CreateAsync(mutations,mp.PageNumber, mp.PageSize);
        }

        public async Task<Mutation> GetMutation(int id)
        {
            return await _context.Mutations 
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Mutation> GetMutation(DateTime created, int userId, int accountId)
        {
            return await _context.Mutations 
                .Where(m => m.Created == created)
                .FirstOrDefaultAsync(m => m.UserId == userId && m.AccountId == accountId);
        }
 

        public async Task<Interest> GetInterest(int id)
        {
            return await _context.Interests 
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.Include(u => u.Accounts)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUser(string username)
        {
            return await _context.Users.Include(u => u.Accounts)
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync()>0;
        }

        public async Task<PagedList<Mutation>> GetMutationsForUserAccount(MutationParams mutationParams)
        {
            var mutations = _context.Mutations
                .Where(m => m.UserId == mutationParams.UserId && m.AccountId == mutationParams.AccountId)
                .AsQueryable();

            var omutations = mutations.OrderByDescending(m => m.Id);
            return await PagedList<Mutation>.CreateAsync(omutations, mutationParams.PageNumber, mutationParams.PageSize);
        }
    }
}