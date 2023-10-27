using AutoMapper;
using Onyx.Infrastructure.Models.Entities;

namespace Onyx.Infrastructure.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<WeatherForecast, WeatherForecastEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Humidity, opt => opt.Ignore())
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Coordinates.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Coordinates.Longitude));

            CreateMap<WeatherForecastEntity, WeatherForecast>()
                .ForPath(dest => dest.Coordinates.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForPath(dest => dest.Coordinates.Longitude, opt => opt.MapFrom(src => src.Longitude));
        }
    }
}