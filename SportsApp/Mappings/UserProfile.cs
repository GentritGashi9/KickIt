using AutoMapper;
using SportsApp.Data;
using SportsApp.Models;
using SportsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(dst => dst.Username, opt => opt.MapFrom(x => x.UserName))
                .ForMember(dst => dst.FullName, opt => opt.MapFrom(x => x.Name + " " + x.Surname))
                //.ForMember(dst => dst.Team, opt => opt.MapFrom(x => x.TeamId))
                .ForMember(dst => dst.Avatar, opt => opt.MapFrom(x => x.ProfileImg));
            CreateMap<UserViewModel, ApplicationUser>();
        }
    }
}
