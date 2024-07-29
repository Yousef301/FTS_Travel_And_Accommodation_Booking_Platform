using MediatR;

namespace TABP.Application.Commands.Amenities.DeleteAmenity;

public class DeleteAmenityCommand : IRequest
{
    public Guid Id { get; set; }
}