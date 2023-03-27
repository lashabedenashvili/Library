using FluentValidation;
using Library.Infrastructure.PropertyValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dto.LibraryDto
{
    public class BookDto
    {
        public string Title { get; set; } 
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal? Rating { get; set; }
    }

    public class BookDtoValidator : AbstractValidator<BookDto>
    {
        private readonly IPropertyValidators _validator;

        public BookDtoValidator(IPropertyValidators validator)
        {
            _validator = validator;

            RuleFor(x => x.Title)
                .NotEmpty()
                .Must(_validator.OnlyLettersValidator)
                .MaximumLength(150)
                .WithMessage(_validator.errNotValidCharacter);

            RuleFor(x => x.Description)
                .NotEmpty()
                .Must(_validator.OnlyLettersValidator)
                .MaximumLength(500)
                .WithMessage(_validator.errNotValidCharacter);
        }
    }
}
