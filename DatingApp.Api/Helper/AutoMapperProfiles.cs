using System.Linq;
using AutoMapper;
using DatingApp.Api.Helper;
using DatingApp.Api.Models;
using DatingApp.Api.Dtos;
using DatingApp.API.Dtos;

namespace DatingApp.Api.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt => {
                    opt.MapFrom(d => d.DateOfBirth.GetAge());
                });
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt => {
                    opt.MapFrom(d => d.DateOfBirth.GetAge());
                });
            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<Photo, PhotoToReturnDto>();
            CreateMap<PhotoForCreationDto,Photo>();
            CreateMap<UserForUpdateDto,User>();
            CreateMap<UserForRegisterDto,User>();
        }
    }
}