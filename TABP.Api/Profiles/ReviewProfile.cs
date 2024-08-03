using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using TABP.Application.Commands.Reviews.CreateReview;
using TABP.Application.Commands.Reviews.UpdateReview;
using TABP.Web.DTOs.Reviews;

namespace TABP.Web.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<CreateReviewDto, CreateReviewCommand>();
        CreateMap<JsonPatchDocument<UpdateReviewDto>, JsonPatchDocument<ReviewUpdate>>();
        CreateMap<Operation<UpdateReviewDto>, Operation<ReviewUpdate>>();
    }
}