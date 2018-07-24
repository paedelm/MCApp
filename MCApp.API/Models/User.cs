using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MCApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public User() {
            Accounts = new Collection<Account>();
        }
                
    }
}