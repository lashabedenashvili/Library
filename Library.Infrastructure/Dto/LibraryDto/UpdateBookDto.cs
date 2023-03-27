using FluentValidation;
using Library.Infrastructure.PropertyValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dto.LibraryDto
{
    public class UpdateBookDto 
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Rating { get; set; }
    }
    public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
    {
        private readonly IPropertyValidators _validator;

        public UpdateBookDtoValidator(IPropertyValidators validator)
        {
            _validator = validator;

            RuleFor(x => x.Title)
                .MaximumLength(150)
                .When(x => !string.IsNullOrEmpty(x.Title))
                .Must(_validator.OnlyLettersValidator)
                .When(x => !string.IsNullOrEmpty(x.Title))
                .WithMessage(_validator.errNotValidCharacter);

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Description))
                .Must(_validator.OnlyLettersValidator)
                .When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage(_validator.errNotValidCharacter);

            RuleFor(x => x.Rating)
              .LessThan(20);
        }
    }
}
