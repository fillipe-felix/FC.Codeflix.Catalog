using FluentValidation;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategoryInputValidator : AbstractValidator<GetCategoryInput>
{
    public GetCategoryInputValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
    }
}
