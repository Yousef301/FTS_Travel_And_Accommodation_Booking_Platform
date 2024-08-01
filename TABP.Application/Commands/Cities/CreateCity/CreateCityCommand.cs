using MediatR;
using TABP.Application.Queries.Cities;

namespace TABP.Application.Commands.Cities.CreateCity;

public class CreateCityCommand : IRequest<CityResponse>
{
    public string Name { get; init; }
    public string Country { get; init; }
}