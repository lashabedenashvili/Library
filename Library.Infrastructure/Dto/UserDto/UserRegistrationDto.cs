using FluentValidation;
using FluentValidation.Validators;
using Library.Infrastructure.PropertyValidator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Data.Domein.Data.Enum;

namespace Library.Infrastructure.Dto.UserDto
{
    public class UserRegistrationDto
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string PrivateNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserRegistrationDtoValidator : AbstractValidator<UserRegistrationDto>
    {
        private readonly IPropertyValidators _validator;

        public UserRegistrationDtoValidator(IPropertyValidators validator)
        {
            _validator = validator;

            RuleFor(x => x.Name)
                .NotEmpty()
                .Must(_validator.OnlyLettersValidator)
                .MaximumLength(50)
                .WithMessage(_validator.errNotValidCharacter);

            RuleFor(x => x.SurName)
                .NotEmpty()
                .Must(_validator.OnlyLettersValidator)
                .MaximumLength(50)
                .WithMessage(_validator.errNotValidCharacter);

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .Must(x => _validator.BeAValidAge(x))
                .WithMessage(_validator.errDateNotCorrext);

            RuleFor(x => x.Gender)
                .IsInEnum()
                .WithMessage(_validator.errNotCorretFormat);

            RuleFor(x => x.PrivateNumber)
                .NotEmpty()
                .Must(_validator.OnlyNumbersValidator)
                .WithMessage(_validator.errNotCorretFormat);


            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Must(_validator.PhoneNumbValidator)
                .WithMessage(_validator.errNotCorretFormat);

            RuleFor(x => x.Email)
                .NotEmpty()
                .Must(_validator.EmailValidator)
                .WithMessage(_validator.errIncorectEmail);

            RuleFor(x => x.Password)
                .NotEmpty()
                .Must(_validator.PasswordValidator)
                .WithMessage(_validator.errIncorectPassword);

        }
    }
}
