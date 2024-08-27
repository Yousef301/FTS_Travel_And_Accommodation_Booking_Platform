using TABP.Shared.Models;

namespace TABP.Shared.Extensions;

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