using MediatR;
using TABP.Domain.Enums;
using TABP.Domain.Models;

namespace TABP.Application.Queries.Hotels.GetHotelsForAdmin;

public class GetHotelsForAdminQuery : IRequest<PagedList<HotelAdminResponse>>
{
    public string? SearchString { get; init; }
    public string? SortBy { get; init; }
    public SortOrder SortOrder { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}