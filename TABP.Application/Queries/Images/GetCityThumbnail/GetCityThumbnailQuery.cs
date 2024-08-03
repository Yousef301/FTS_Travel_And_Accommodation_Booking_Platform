using MediatR;

namespace TABP.Application.Queries.Images.GetCityThumbnail;

public class GetCityThumbnailQuery : IRequest<ImageResponse>
{
    public Guid CityId { get; set; }
}