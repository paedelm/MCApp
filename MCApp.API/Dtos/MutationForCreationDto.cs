using System.ComponentModel.DataAnnotations;

namespace MCApp.API.Dtos
{
    public class MutationForCreationDto
    {
        [Required]
        public string Accountname { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public int AccountId { get; set; }
        [Required]
        public double Amount { get; set; }        
    }
}