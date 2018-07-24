using System;

namespace MCApp.API.Dtos
{
    public class MutationForDetailedDto
    {
        public int Id { get; set; }
        public AccountForDetailedDto Account { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }        
        
    }
}