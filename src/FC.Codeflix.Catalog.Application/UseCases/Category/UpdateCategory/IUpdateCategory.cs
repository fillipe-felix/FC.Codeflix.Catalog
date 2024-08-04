using FC.Codeflix.Catalog.Application.UseCases.Category.Common;

using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

public interface IUpdateCategory : IRequestHandler<UpdateCategoryInput, CategoryModelOutput>
{
    Task<CategoryModelOutput> Handle(UpdateCategoryInput input, CancellationToken cancellationToken);
}
