using MediatR;

namespace TABP.Application.Queries.Images.Hotels.GetHotelImages;

public class GetHotelImagesQuery : IRequest<IEnumerable<Dictionary<string, string>>>
{
    public Guid HotelId { get; set; }
}