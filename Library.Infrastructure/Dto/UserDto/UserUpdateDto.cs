using FluentValidation;
using Library.Infrastructure.PropertyValidator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dto.UserDto
{
    public class UserUpdateDto
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        private readonly IPropertyValidators _validator;

        public UserUpdateDtoValidator(IPropertyValidators validator)
        {
            _validator = validator;

            RuleFor(x => x.Name)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .Must(_validator.OnlyLettersValidator)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(_validator.errNotValidCharacter);

            RuleFor(x => x.SurName)
                  .MaximumLength(50)
                  .When(x => !string.IsNullOrEmpty(x.SurName))
                  .Must(_validator.OnlyLettersValidator)
                  .When(x => !string.IsNullOrEmpty(x.SurName))
                  .WithMessage(_validator.errNotValidCharacter);

            RuleFor(x => x.BirthDate)
                .NotNull()
                .Must(date => _validator.BeAValidAge(date))
                .When(x => x.BirthDate != null)
                .WithMessage(_validator.errDateNotCorrext);

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .Must(_validator.OnlyNumbersValidator)
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage(_validator.errNotValidCharacter);

        }
    }
}
