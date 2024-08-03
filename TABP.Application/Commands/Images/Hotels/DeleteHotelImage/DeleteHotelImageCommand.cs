using MediatR;

namespace TABP.Application.Commands.Images.Hotels.DeleteHotelImage;

public class DeleteHotelImageCommand : IRequest
{
    public Guid ImageId { get; set; }
}