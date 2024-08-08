using MediatR;
using TABP.Domain.Enums;
using TABP.Domain.Models;

namespace TABP.Application.Queries.Hotels.GetHotelsForAdmin;

public class GetHotelsForAdminQuery : IRequest<PagedList<HotelAdminResponse>>
{
    public string? SearchString { get; init; }
    public string? SortBy { get; set; }
    public SortOrder SortOrder { get; set; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}