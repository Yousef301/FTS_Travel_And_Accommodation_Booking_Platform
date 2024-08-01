using AutoMapper;
using MediatR;
using TABP.Application.Queries.Cities;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Cities.CreateCity;

public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, CityResponse>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCityCommandHandler(ICityRepository cityRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    public async Task<CityResponse> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var city = _mapper.Map<City>(request);

        city.Id = new Guid();

        var createdCity = await _cityRepository.CreateAsync(city);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CityResponse>(createdCity);
    }
}