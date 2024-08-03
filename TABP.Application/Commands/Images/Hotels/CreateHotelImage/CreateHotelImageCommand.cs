using MediatR;
using Microsoft.AspNetCore.Http;

namespace TABP.Application.Commands.Images.Hotels.CreateHotelImage;

public class CreateHotelImageCommand : IRequest
{
    public Guid HotelId { get; set; }
    public List<IFormFile> Images { get; set; }
}