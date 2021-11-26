using AutoMapper;
using RestaurantApp.Data.Models.Users;
using RestaurantApp.MVC.ViewModels.Accounts;
using RestaurantApp.MVC.ViewModels.Users;

namespace RestaurantApp.MVC.Infrastructure.Mappings
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            //From view models to Domain
            this.CreateMap<AddUserViewModel, User>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email));
            this.CreateMap<RegisterViewModel, User>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email));

            //From Domain to view models
            this.CreateMap<User, EditUserViewModel>();
        }
    }
}
