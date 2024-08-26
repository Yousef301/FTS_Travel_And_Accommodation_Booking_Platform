using AutoMapper;
using MediatR;
using TABP.Application.Queries.Cities;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Cities.CreateCity;

public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, CityResponse>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCityCommandHandler(ICityRepository cityRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    public async Task<CityResponse> Handle(CreateCityCommand request,
        CancellationToken cancellationToken)
    {
        if (await _cityRepository.ExistsAsync(c =>
                c.Name.ToLower() == request.Name.ToLower() &&
                c.Country.ToLower() == request.Country.ToLower()))
        {
            throw new UniqueConstraintViolationException($"The city is already exist.");
        }

        var city = _mapper.Map<City>(request);

        var createdCity = await _cityRepository.CreateAsync(city);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CityResponse>(createdCity);
    }
}