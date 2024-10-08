﻿using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Exceptions;

namespace TABP.Application.Queries.Reviews.GetHotelReviews;

public class GetHotelReviewsQueryHandler : IRequestHandler<GetHotelReviewsQuery, IEnumerable<ReviewResponse>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelReviewsQueryHandler(IMapper mapper,
        IReviewRepository reviewRepository,
        IHotelRepository hotelRepository)
    {
        _mapper = mapper;
        _reviewRepository = reviewRepository;
        _hotelRepository = hotelRepository;
    }

    public async Task<IEnumerable<ReviewResponse>> Handle(GetHotelReviewsQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId))
        {
            throw new NotFoundException(nameof(Hotel), request.HotelId);
        }

        var reviews = await _reviewRepository
            .GetByHotelIdAsync(request.HotelId);

        return _mapper.Map<IEnumerable<ReviewResponse>>(reviews);
    }
}