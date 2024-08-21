using MediatR;

namespace TABP.Application.Queries.Hotels.GetHotelById;

public class GetHotelByIdQuery : IRequest<HotelResponseBase>
{
    public Guid Id { get; set; }
}