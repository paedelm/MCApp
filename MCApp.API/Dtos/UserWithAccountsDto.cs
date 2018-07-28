namespace MCApp.API.Dtos
{
    public class UserWithAccountsDto
    {
        public int id { get; set; }
        public string Username { get; set; }
        public string KnownAs { get; set; }
        public AccountForListDto[] Accounts { get; set; }

    }
}