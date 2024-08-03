using MediatR;
using Microsoft.AspNetCore.Http;

namespace TABP.Application.Commands.Images.Hotels.CreateHotelThumbnail;

public class CreateHotelThumbnailCommand : IRequest
{
    public Guid HotelId { get; set; }
    public IFormFile Image { get; set; }
}