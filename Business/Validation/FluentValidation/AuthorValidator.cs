using Core.Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Validation.FluentValidation
{
    public class AuthorValidator:AbstractValidator<Author>
    {
        public AuthorValidator()
        {
            RuleFor(p => p.FirstName).NotNull().NotEmpty();
            RuleFor(p => p.LastName).NotNull().NotEmpty();
            RuleFor(p => p.Mail).EmailAddress();
        }
    }
}
