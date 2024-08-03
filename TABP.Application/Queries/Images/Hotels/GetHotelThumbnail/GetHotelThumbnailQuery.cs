using MediatR;

namespace TABP.Application.Queries.Images.Hotels.GetHotelThumbnail;

public class GetHotelThumbnailQuery : IRequest<ImageResponse>
{
    public Guid HotelId { get; set; }
}