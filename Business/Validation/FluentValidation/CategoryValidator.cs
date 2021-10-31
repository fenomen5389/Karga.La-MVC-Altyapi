using Business.Constants;
using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Validation.FluentValidation
{
    public class CategoryValidator:AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(p => p.Name).NotNull().NotEmpty().WithMessage(Messages.CategoryNameCannotBeEmpty);
        }
    }
}
