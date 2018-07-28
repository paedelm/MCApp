using AutoMapper;
using MCApp.API.Dtos;
using MCApp.API.Models;

namespace MCApp.API.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() {
            CreateMap<User, UserForAccountDto>();
            CreateMap<UserForRegisterDto, User>();
            
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.Age, opt => opt.ResolveUsing(src => src.DateOfBirth.CalculateAge()));
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.Age, opt => opt.ResolveUsing(src => src.DateOfBirth.CalculateAge()));
            
            CreateMap<Account, AccountForListDto>();
            CreateMap<Account, AccountForDetailedDto>();
            CreateMap<AccountForCreationDto, Account>();

            CreateMap<Mutation, MutationForDetailedDto>();
            CreateMap<MutationForCreationDto, Mutation>();

            CreateMap<Interest, InterestForDetailedDto>();
            CreateMap<InterestForCreationDto, Interest>();

            CreateMap<User, UserWithAccountsDto>();
        }
    }
}