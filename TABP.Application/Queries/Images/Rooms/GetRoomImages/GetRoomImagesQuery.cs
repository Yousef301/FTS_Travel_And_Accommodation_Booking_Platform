using MediatR;

namespace TABP.Application.Queries.Images.Rooms.GetRoomImages;

public class GetRoomImagesQuery : IRequest<IEnumerable<string>>
{
    public Guid RoomId { get; set; }
}