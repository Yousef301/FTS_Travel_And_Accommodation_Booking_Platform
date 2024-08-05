using MediatR;

namespace TABP.Application.Queries.Images.Cities.GetCityImages;

public class GetCityImagesQuery : IRequest<IEnumerable<Dictionary<string, string>>>
{
    public Guid CityId { get; set; }
}