using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCApp.API.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Accountname { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastInterestMutation { get; set; }
        public string InterestPeriod { get; set; }
        public DateTime LastActive { get; set; }
        public double Percentage { get; set; }
        public double CalculatedInterest { get; set; }
        public double Balance { get; set; }
        public User User { get; set; }
        public int UserId { get; set; } 
        [NotMapped]
        public Mutation LastMutation { get; set; }
        public DateTime LastMutationCreated { get; set; }
        public ICollection<Mutation> Mutations { get; set; }
        public ICollection<Interest> Interests { get; set; }
        public Account() {
            Mutations = new Collection<Mutation>();
            Interests = new Collection<Interest>();
            Created = DateTime.Now;
            LastActive = Created;
            LastInterestMutation = Created;
            InterestPeriod = "year";
            CalculatedInterest = 0F;
        }
        
    }
}