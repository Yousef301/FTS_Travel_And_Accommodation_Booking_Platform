using Microsoft.AspNetCore.Http.Extensions;
using TABP.Domain.Models;

namespace TABP.Web.Extensions;

public static class HttpResponseExtension
{
    public static void AddPaginationMetadata(this HttpResponse response, PaginationMetadata paginationMetadata,
        HttpRequest request)
    {
        var baseUrl = request.GetDisplayUrl();
        var uriBuilder = new UriBuilder(baseUrl)
        {
            Query = null
        };
        var baseUri = uriBuilder.ToString().TrimEnd('/');

        var nextPageUrl = paginationMetadata.HasNextPage
            ? $"{baseUri}?page={paginationMetadata.Page + 1}&pageSize={paginationMetadata.PageSize}"
            : null;
        var previousPageUrl = paginationMetadata.Page > 1 && paginationMetadata.HasPreviousPage
            ? $"{baseUri}?page={paginationMetadata.Page - 1}&pageSize={paginationMetadata.PageSize}"
            : null;

        response.Headers.Add("X-Total-Count", paginationMetadata.TotalCount.ToString());
        response.Headers.Add("X-Limit", paginationMetadata.PageSize.ToString());

        if (paginationMetadata.HasNextPage)
        {
            response.Headers.Add("X-Next-Page-Url", nextPageUrl);
        }

        if (previousPageUrl != null)
        {
            response.Headers.Add("X-Previous-Page-Url", previousPageUrl);
        }
    }
}