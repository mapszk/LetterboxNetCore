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
            CreateMap<User, UserReviewDTO>()
                .ForMember(u => u.UserId, options => options.MapFrom(u => u.Id));

            CreateMap<CreateMovieDTO, Movie>();
            CreateMap<UpdateMovieDTO, Movie>();
            CreateMap<Movie, MovieDTO>();
            CreateMap<Movie, MovieDetailsDTO>()
                .ForMember(m => m.Likes, options => options.MapFrom(m => m.Likes.Count()))
                .ForMember(m => m.Watchlist, options => options.MapFrom(m => m.MovieWatchlist.Count()))
                .ForMember(m => m.Reviews, options => options.MapFrom(m => m.Reviews.Count()));

            CreateMap<CreateReviewDTO, Review>();
            CreateMap<Review, ReviewDTO>()
                .ForMember(r => r.UserInfo, options => options.MapFrom(m => m.User))
                .ForMember(r => r.MovieInfo, options => options.MapFrom(m => m.Movie));
        }
    }
}