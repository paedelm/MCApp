using System;

namespace MCApp.API.Dtos
{
    public class AccountForListDto
    {
        public int Id { get; set; }
        public string Accountname { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }        
        
    }
}