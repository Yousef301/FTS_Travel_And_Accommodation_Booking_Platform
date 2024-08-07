using MediatR;
using TABP.Domain.Models;

namespace TABP.Application.Queries.Hotels.GetHotels;

public class GetHotelsQuery : IRequest<PagedList<HotelResponse>>
{
    public int Page { get; init; }
    public int PageSize { get; init; }
}