using TABP.Domain.Enums;

namespace TABP.Domain.Models;

public class Query
{
    public string SortColumn { get; init; }
    public SortOrder SortOrder { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}