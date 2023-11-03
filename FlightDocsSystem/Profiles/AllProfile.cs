using AutoMapper;
using FlightDocsSystem.Models.ViewModel;

namespace FlightDocsSystem.Profiles
{
    public class AllProfile : Profile
    {
        public AllProfile()
        {
            CreateMap<Group, GroupVM>();
            CreateMap<GroupVM, Group>();
            //CreateMap<GroupModel, Group>();
            //CreateMap<Group, GroupModel>();

            CreateMap<User, UserVM>();
            CreateMap<UserVM, User>();

            CreateMap<Flight, FlightVM>();
            CreateMap<FlightVM, Flight>();
        }
    }
}
