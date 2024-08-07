using System.Linq.Expressions;
using TABP.Domain.Enums;

namespace TABP.Domain.Models;

public class Query<T>
{
    public Expression<Func<T, bool>>? Expression { get; init; }
    public string? SearchString { get; init; }
    public string SortColumn { get; init; }
    public SortOrder SortOrder { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}