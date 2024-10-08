﻿using AutoMapper;
using MediatR;
using TABP.Application.Queries.SpecialOffers;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Exceptions;

namespace TABP.Application.Commands.SpecialOffers.UpdateSpecialOffer;

public class UpdateSpecialOfferCommandHandler : IRequestHandler<UpdateSpecialOfferCommand, SpecialOfferResponse>
{
    private readonly ISpecialOfferRepository _specialOfferRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateSpecialOfferCommandHandler(ISpecialOfferRepository specialOfferRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _specialOfferRepository = specialOfferRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SpecialOfferResponse> Handle(UpdateSpecialOfferCommand request,
        CancellationToken cancellationToken)
    {
        var offer = await _specialOfferRepository
                        .GetByRoomIdAndOfferIdAsync(request.Id, request.RoomId) ??
                    throw new NotFoundException("Offer", request.Id);

        var updatedOfferDto = _mapper.Map<SpecialOfferUpdate>(offer);

        request.SpecialOfferDocument.ApplyTo(updatedOfferDto);

        _mapper.Map(updatedOfferDto, offer);

        _specialOfferRepository.Update(offer);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SpecialOfferResponse>(offer);
    }
}