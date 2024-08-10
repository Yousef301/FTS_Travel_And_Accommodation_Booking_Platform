using MediatR;

namespace TABP.Application.Commands.Images.Cities.DeleteCityImage;

public class DeleteCityImageCommand : IRequest
{
    public Guid CityId { get; set; }
    public Guid ImageId { get; set; }
}