using FluentValidation;
using Library.Infrastructure.PropertyValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dto.LibraryDto
{
    public class AuthorDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class AuthorDtoValidator : AbstractValidator<AuthorDto>
    {
        private readonly IPropertyValidators _validator;

        public AuthorDtoValidator(IPropertyValidators validator)
        {
            _validator = validator;

            RuleFor(x => x.Name)
                .NotEmpty()
                .Must(_validator.OnlyLettersValidator)
                .MaximumLength(50)
                .WithMessage(_validator.errNotValidCharacter);

            RuleFor(x => x.Surname)
                .NotEmpty()
                .Must(_validator.OnlyLettersValidator)
                .MaximumLength(50)
                .WithMessage(_validator.errNotValidCharacter);

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .Must(x => _validator.BeAValidAge(x))
                .WithMessage(_validator.errDateNotCorrext);
        }
    }
}
