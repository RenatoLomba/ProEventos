using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Models;

namespace ProEventos.Application.Helpers
{
    public class Mappers
    {
        public static void ConfigMappers(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Event, EventDto>().ReverseMap();
            cfg.CreateMap<Batch, BatchDto>().ReverseMap();
            cfg.CreateMap<SocialNetwork, SocialNetworkDto>().ReverseMap();
            cfg.CreateMap<Speaker, SpeakerDto>().ReverseMap();
            cfg.CreateMap<User, UserDto>().ReverseMap();
            cfg.CreateMap<User, UserLoginDto>().ReverseMap();
            cfg.CreateMap<User, UserUpdateDto>().ReverseMap();
        }
    }
}
