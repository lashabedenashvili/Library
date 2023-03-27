using FluentValidation;
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
        public UserLogInDtoValidator()
        {
            RuleFor(x => x.Email).MaximumLength(3);
        }
    }
}
