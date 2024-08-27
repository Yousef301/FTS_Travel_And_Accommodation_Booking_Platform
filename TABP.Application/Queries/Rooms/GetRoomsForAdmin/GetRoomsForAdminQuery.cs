using MediatR;
using TABP.Shared.Enums;
using TABP.Shared.Models;

namespace TABP.Application.Queries.Rooms.GetRoomsForAdmin;

public class GetRoomsForAdminQuery : IRequest<PagedList<RoomAdminResponse>>
{
    public Guid HotelId { get; set; }
    public string? SearchString { get; init; }
    public string? SortBy { get; set; }
    public SortOrder SortOrder { get; set; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}