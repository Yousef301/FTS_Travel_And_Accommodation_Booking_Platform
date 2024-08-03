using MediatR;

namespace TABP.Application.Commands.Images.Cities.DeleteCityImage;

public class DeleteCityImageCommand : IRequest
{
    public Guid ImageId { get; set; }
}