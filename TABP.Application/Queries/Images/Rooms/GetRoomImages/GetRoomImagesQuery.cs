using MediatR;

namespace TABP.Application.Queries.Images.Rooms.GetRoomImages;

public class GetRoomImagesQuery : IRequest<IEnumerable<Dictionary<string, string>>>
{
    public Guid RoomId { get; set; }
}