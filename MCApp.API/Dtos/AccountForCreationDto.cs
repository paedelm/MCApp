using System.ComponentModel.DataAnnotations;

namespace MCApp.API.Dtos
{
    public class AccountForCreationDto
    {
        [Required]
        public string Accountname { get; set; }
        [Required]
        public double Percentage { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        
    }
}