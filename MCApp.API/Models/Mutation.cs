using System;

namespace MCApp.API.Models
{
    public class Mutation
    {
        public int Id { get; set; }
        public int PrevId { get; set; }
        public DateTime Created { get; set; }
        public DateTime InterestDate { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public Account Account { get; set; }
        public string Description { get; set; }
        public int AccountId { get; set; } 
        public User User { get; set; }
        public int UserId { get; set; }
        public Mutation()
        {
            Created = DateTime.Now;
            InterestDate = Created;
        }      
    }
}