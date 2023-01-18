using AutoMapper;
using LetterboxNetCore.DTOs;
using LetterboxNetCore.Models;

namespace LetterboxNetCore.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserRegisterDTO, User>();

            CreateMap<CreateMovieDTO, Movie>();
        }
    }
}