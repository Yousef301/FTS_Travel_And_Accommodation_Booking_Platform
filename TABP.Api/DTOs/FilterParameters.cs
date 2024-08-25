using TABP.Domain.Constants;

namespace TABP.Web.DTOs;

public class FilterParameters
{
    public string? SearchString { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
    public int Page { get; set; } = Constants.DefaultPage;
    public int PageSize { get; set; } = Constants.DefaultPageSize;
}