using MediatR;

namespace TABP.Application.Commands.Cities.DeleteCity;

public class DeleteCityCommand : IRequest
{
    public Guid Id { get; init; }
}