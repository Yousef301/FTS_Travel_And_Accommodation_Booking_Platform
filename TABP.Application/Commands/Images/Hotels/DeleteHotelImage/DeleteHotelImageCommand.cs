using MediatR;

namespace TABP.Application.Commands.Images.Hotels.DeleteHotelImage;

public class DeleteHotelImageCommand : IRequest
{
    public Guid HotelId { get; set; }
    public Guid ImageId { get; set; }
}