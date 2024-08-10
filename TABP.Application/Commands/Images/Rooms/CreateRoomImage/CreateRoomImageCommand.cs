using MediatR;
using Microsoft.AspNetCore.Http;

namespace TABP.Application.Commands.Images.Rooms.CreateRoomImage;

public class CreateRoomImageCommand : IRequest
{
    public Guid HotelId { get; set; }
    public Guid RoomId { get; set; }
    public List<IFormFile> Images { get; set; }
}