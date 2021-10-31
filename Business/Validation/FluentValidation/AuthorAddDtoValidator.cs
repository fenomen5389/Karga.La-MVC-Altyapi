using Business.Constants;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Validation.FluentValidation
{
    public class AuthorAddDtoValidator:AbstractValidator<AuthorAddDto>
    {
        public AuthorAddDtoValidator()
        {
            RuleFor(p => p.Mail).EmailAddress().WithMessage("Mail geçersiz.");
            RuleFor(p => p.FirstName).NotNull();
            RuleFor(p => p.LastName).NotNull();
            RuleFor(p => p.Password).NotNull();
        }
    }
}
