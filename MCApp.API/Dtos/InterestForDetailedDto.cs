using System;

namespace MCApp.API.Dtos
{
    public class InterestForDetailedDto
    {
        public int Id { get; set; }
        public AccountForDetailedDto Account { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public double Percentage { get; set; }
        
    }
}