using FluentValidation;
using Library.Infrastructure.PropertyValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dto.UserDto
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        private readonly IPropertyValidators _validator;

        public ChangePasswordDtoValidator(IPropertyValidators validator)
        {
            _validator = validator;

            RuleFor(x => x.OldPassword)
                .Must(x => _validator.PasswordValidator(x))
                .WithMessage(_validator.errIncorectPassword);

            RuleFor(x => x.NewPassword)
                .Must(x => _validator.PasswordValidator(x))
                .WithMessage(_validator.errNotCorretFormat);
        }
    }
}
