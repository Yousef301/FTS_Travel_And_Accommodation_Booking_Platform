using MediatR;

namespace TABP.Application.Queries.Images.Hotels.GetHotelImageById;

public class GetHotelImageByIdQuery : IRequest<ImageResponse>
{
    public Guid ImageId { get; set; }
}