using TABP.Domain.Enums;

namespace TABP.Web.DTOs;

public class FilterParameters
{
    public string? SearchString { get; set; }
    public string? SortBy { get; set; }
    public SortOrder SortOrder { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}