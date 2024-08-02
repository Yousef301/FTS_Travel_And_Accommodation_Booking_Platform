using AutoMapper;
using TABP.Application.Commands.Reviews.CreateReview;
using TABP.Application.Commands.Reviews.UpdateReview;
using TABP.Application.Queries.Reviews;
using TABP.DAL.Entities;

namespace TABP.Application.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<CreateReviewCommand, Review>();
        CreateMap<Review, ReviewUpdate>();
        CreateMap<ReviewUpdate, Review>();
        CreateMap<Review, ReviewResponse>()
            .ForMember(dest => dest.Username,
                opt =>
                    opt.MapFrom(src => src.User.LastName));
    }
}