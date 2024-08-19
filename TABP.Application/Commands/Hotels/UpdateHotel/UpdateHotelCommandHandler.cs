using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Hotels.UpdateHotel;

public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateHotelCommandHandler(IHotelRepository hotelRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateHotelCommand request,
        CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.Id) ??
                    throw new NotFoundException(nameof(Hotel), request.Id);

        var updatedHotelDto = _mapper.Map<HotelUpdate>(hotel);

        request.HotelDocument.ApplyTo(updatedHotelDto);

        _mapper.Map(updatedHotelDto, hotel);

        _hotelRepository.Update(hotel);

        await _unitOfWork.SaveChangesAsync();
    }
}