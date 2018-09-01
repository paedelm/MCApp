using System;

namespace MCApp.API.Dtos
{
    public class MutationForListDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }        
        
        
    }
}