using MediatR;
using TABP.Shared.Enums;
using TABP.Shared.Models;

namespace TABP.Application.Queries.Cities.GetCitiesForAdmin;

public class GetCitiesForAdminQuery : IRequest<PagedList<CityAdminResponse>>
{
    public string? SearchString { get; init; }
    public string? SortBy { get; set; }
    public SortOrder SortOrder { get; set; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}