using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Cities.UpdateCity;

public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCityCommandHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.Id);

        if (city == null)
        {
            throw new Exception("City not found"); // TODO: Create a custom exception
        }

        var updatedCityDto = _mapper.Map<CityUpdate>(city);

        request.CityDocument.ApplyTo(updatedCityDto);

        _mapper.Map(updatedCityDto, city);

        await _cityRepository.UpdateAsync(city);

        await _unitOfWork.SaveChangesAsync();
    }
}