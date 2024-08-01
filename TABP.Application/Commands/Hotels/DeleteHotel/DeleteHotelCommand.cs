using MediatR;

namespace TABP.Application.Commands.Hotels.DeleteHotel;

public class DeleteHotelCommand : IRequest
{
    public Guid Id { get; set; }
}