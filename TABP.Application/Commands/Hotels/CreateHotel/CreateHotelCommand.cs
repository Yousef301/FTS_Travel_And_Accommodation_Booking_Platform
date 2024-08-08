using MediatR;
using TABP.Application.Queries.Hotels;

namespace TABP.Application.Commands.Hotels.CreateHotel;

public class CreateHotelCommand : IRequest<HotelUserResponse>
{
    public Guid CityId { get; set; }
    public string Name { get; set; }
    public string Owner { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public double Rating { get; set; }
}