using MediatR;

namespace TABP.Application.Commands.Images.DeleteCityImage;

public class DeleteCityImageCommand : IRequest
{
    public Guid ImageId { get; set; }
}