using System.Collections.Generic;

namespace MCApp.API.Dtos
{
    public class MutationForPageDto
    {
        public AccountForDetailedDto Account { get; set; }
        public ICollection<MutationForListDto> Mutations { get; set; }
        
    }
}