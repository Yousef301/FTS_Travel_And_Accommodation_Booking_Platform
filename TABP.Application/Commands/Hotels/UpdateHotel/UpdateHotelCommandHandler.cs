using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Hotels.UpdateHotel;

public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateHotelCommandHandler(IHotelRepository hotelRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.Id);

        if (hotel == null)
        {
            throw new Exception("Hotel not found"); // TODO: Create a custom exception
        }

        var updatedHotelDto = _mapper.Map<HotelUpdate>(hotel);

        request.HotelDocument.ApplyTo(updatedHotelDto);

        _mapper.Map(updatedHotelDto, hotel);

        await _hotelRepository.UpdateAsync(hotel);

        await _unitOfWork.SaveChangesAsync();
    }
}