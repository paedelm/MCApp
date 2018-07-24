using System;

namespace MCApp.API.Models
{
    public class Interest
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime InterestDate { get; set; }
        public string Description { get; set; }
        public double Percentage { get; set; }
        public Account Account { get; set; }
        public int AccountId { get; set; } 
        public User User { get; set; }
        public int UserId { get; set; }
        public Interest()
        {
            Created = DateTime.Now;
            InterestDate = Created;
        }      
        
    }
}