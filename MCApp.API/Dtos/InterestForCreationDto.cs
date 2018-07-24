using System.ComponentModel.DataAnnotations;

namespace MCApp.API.Dtos
{
    public class InterestForCreationDto
    {
        [Required]
        public string Accountname { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int AccountId { get; set; }
        [Required]
        public double Percentage { get; set; }        
        
    }
}