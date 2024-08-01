using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace TABP.Application.Commands.Hotels.UpdateHotel;

public class UpdateHotelCommand : IRequest
{
    public Guid Id { get; set; }
    public JsonPatchDocument<HotelUpdate> HotelDocument { get; init; }
}