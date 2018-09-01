using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MCApp.API.Data;
using MCApp.API.Dtos;
using MCApp.API.Helpers;
using MCApp.API.Models;
// using Moq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xunit;

using MCApp.API.Controllers;
using System.Collections.ObjectModel;

namespace MCApp.APITests.Controllers
{
    public class MutationsControllerTests
    {
        [Fact]
        public void MapperTests()
        {
        //Given
            Mapper.Initialize( cfg => cfg.AddProfile<AutoMapperProfile>());
            var user = new User { KnownAs="Peter", Username="paedelm", City="Hilversum",
             Country="The Netherlands", Accounts= new Collection<Account> {
                  new Account { Accountname="pensioen" },
                  new Account { Accountname="fiets" } 
                  } };
            user.KnownAs = "Peter";
            user.Username = "paedelm";
        //When
            var userForDetailed = Mapper.Map<UserForDetailedDto>(user); 
        
        //Then
            Assert.Equal(user.Accounts.Count, userForDetailed.Accounts.Count);
        }
    }
}