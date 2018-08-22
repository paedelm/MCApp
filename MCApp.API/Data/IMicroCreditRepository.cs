using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MCApp.API.Helpers;
using MCApp.API.Models;

namespace MCApp.API.Data
{
    public interface IMicroCreditRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<User> GetUser(int id);
         Task<User> GetUser(string userId);
         Task<Account> GetAccount(int id);
         Task<Account> GetAccount(int userId, string name);
         Task<Mutation> GetMutation(int id);
         Task<Mutation> GetMutation(DateTime created, int userId, int accountId);
         Task<PagedList<Mutation>> GetMutationsForUserAccount(MutationParams mutationParams);
         Task<Interest> GetInterest(int id);
         Task<PagedList<Mutation>> GetAccountMutationsForUser(int userId, int accountId, MutationParams mp);
        
    }
}