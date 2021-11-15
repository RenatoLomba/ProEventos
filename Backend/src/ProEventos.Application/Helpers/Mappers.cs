using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Domain;

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
        }
    }
}
