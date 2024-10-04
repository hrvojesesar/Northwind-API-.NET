using FluentValidation;
using REST_API.Commands.Category;

namespace REST_API.Validators.Category;

public class AddCategoryValidator : AbstractValidator<AddCategoryRequest>
{
    public AddCategoryValidator()
    {
        RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category Name is required")
            .MaximumLength(15).WithMessage("Category Name must not exceed 15 characters")
            .NotNull().WithMessage("Category Name is required");
    }
}

