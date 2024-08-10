using FluentValidation;
using TABP.Web.DTOs;
using TABP.Web.Extensions;

namespace TABP.Web.Validators.Filters;

public class FilterParametersValidator : AbstractValidator<FilterParameters>
{
    public FilterParametersValidator()
    {
        RuleFor(x => x.SearchString)
            .MaximumLength(50).WithMessage("Search value must be less than 50 characters.");

        RuleFor(x => x.SortBy)
            .MaximumLength(25).WithMessage("Sort by must be less than 25 characters.");

        RuleFor(x => x.SortOrder)
            .Must(BeAValidSortOrder)
            .When(x => !string.IsNullOrEmpty(x.SortOrder))
            .WithMessage("SortOrder must be 'asc' or 'desc'.");
        
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");
        
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }

    private bool BeAValidSortOrder(string? sortOrder)
    {
        return sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
               sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);
    }
}