using TABP.Domain.Models;

namespace TABP.Domain.Extensions;

public static class PagedListExtensions
{
    public static PaginationMetadata ToMetadata<T>(this PagedList<T> pagedList)
    {
        return new PaginationMetadata
        {
            Page = pagedList.Page,
            PageSize = pagedList.PageSize,
            TotalCount = pagedList.TotalCount,
            HasNextPage = pagedList.HasNextPage,
            HasPreviousPage = pagedList.HasPreviousPage
        };
    }
}