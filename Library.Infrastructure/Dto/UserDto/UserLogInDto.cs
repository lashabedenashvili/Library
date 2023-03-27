using FluentValidation;
using Library.Infrastructure.PropertyValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dto.UserDto
{
    public class UserLogInDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLogInDtoValidator : AbstractValidator<UserLogInDto>
    {
        private readonly IPropertyValidators _validator;
        public UserLogInDtoValidator(IPropertyValidators validator)
        {
            _validator = validator;
            RuleFor(x => x.Email)
                .NotEmpty()
                .Must(_validator.EmailValidator)
                .WithMessage(_validator.errIncorectEmail)
                .MaximumLength(50);


            RuleFor(x => x.Password)
                .NotEmpty()
                .Must(_validator.PasswordValidator)
                .WithMessage(_validator.errIncorectPassword)
                .MaximumLength(50);
                

        }
    }
}
