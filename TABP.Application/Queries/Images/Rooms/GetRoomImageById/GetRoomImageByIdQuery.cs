using MediatR;

namespace TABP.Application.Queries.Images.Rooms.GetRoomImageById;

public class GetRoomImageByIdQuery : IRequest<ImageResponse>
{
    public Guid ImageId { get; set; }
}