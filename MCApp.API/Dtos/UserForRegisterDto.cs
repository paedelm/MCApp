using System;
using System.ComponentModel.DataAnnotations;

namespace MCApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        [StringLength(24,MinimumLength = 2)]
        public string Username { get; set; }
        [Required]
        [StringLength(12,MinimumLength = 6, ErrorMessage = "minimum Passwordlength is 6 and the maximum is 12")]
        public string Password { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
        
    }
}