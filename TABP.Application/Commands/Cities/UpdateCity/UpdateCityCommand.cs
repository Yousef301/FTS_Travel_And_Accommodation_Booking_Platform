using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace TABP.Application.Commands.Cities.UpdateCity;

public class UpdateCityCommand : IRequest
{
    public Guid Id { get; set; }
    public JsonPatchDocument<CityUpdate> CityDocument { get; init; }
}