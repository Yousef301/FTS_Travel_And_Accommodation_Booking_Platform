using MediatR;

namespace TABP.Application.Queries.Images.GetCityImageById;

public class GetCityImageByIdQuery : IRequest<ImageResponse>
{
    public Guid ImageId { get; set; }
}