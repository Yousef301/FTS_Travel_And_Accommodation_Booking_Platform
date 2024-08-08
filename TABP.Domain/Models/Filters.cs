using System.Linq.Expressions;
using TABP.Domain.Enums;

namespace TABP.Domain.Models;

public class Filters<T>
{
    public Expression<Func<T, bool>>? FilterExpression { get; init; }
    public Expression<Func<T, object>>? SortExpression { get; init; }
    public SortOrder SortOrder { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}