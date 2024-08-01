using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.Domain.Entity;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Category(string name, string description, bool isActive = true)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;
        
        Validate();
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new EntityValidationException($"{nameof(Name)} should not be empty or null");
        }
        
        if (Name.Length < 3)
        {
            throw new EntityValidationException($"{nameof(Name)} should be at leats 3 chatacters long");
        }
        
        if (Name.Length > 255)
        {
            throw new EntityValidationException($"{nameof(Name)} should be less or equal 255 chatacters long");
        }
        
        if (Description is null)
        {
            throw new EntityValidationException($"{nameof(Description)} should not be empty or null");
        }
        
        if (Description.Length > 10000)
        {
            throw new EntityValidationException($"{nameof(Description)} should be less or equal 10.000 chatacters long");
        }
    }
}
