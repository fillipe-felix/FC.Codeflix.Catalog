using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public interface IGetCategory : IRequestHandler<GetCategoryInput, GetCategoryOutput>
{
    Task<GetCategoryOutput> Handle(GetCategoryInput input, CancellationToken cancellationToken);
}
