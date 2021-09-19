using AutoMapper;
using SportsApp.Models;
using SportsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsApp.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<GameRoomChat, RoomViewModel>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(x => x.ChatName))
                .ForMember(dst => dst.RoomImg, opt => opt.MapFrom(x => x.ChatImg));

            CreateMap<RoomViewModel, GameRoomChat>();
        }
    }
}
