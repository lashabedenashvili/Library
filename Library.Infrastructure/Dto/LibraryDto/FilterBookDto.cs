using FluentValidation;
using FluentValidation.Validators;
using Library.Infrastructure.PropertyValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dto.LibraryDto
{
    public class FilterBookDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? RatingFrom { get; set; }
        public decimal? RatingTo { get; set; }
        public bool? InLibrary { get; set; }
        public int PageSize { get; set; } = 20;
        public int PageNumb { get; set; } = 1;
    }
    public class FilterBookDtoValidator : AbstractValidator<FilterBookDto>
    {
        private readonly IPropertyValidators _validator;

        public FilterBookDtoValidator(IPropertyValidators validator)
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

            RuleFor(x => x.PageSize)
                .LessThan(50);

            RuleFor(x => x.PageNumb)
                .LessThan(50);

            RuleFor(x => x.RatingFrom)
               .GreaterThan(0);

            RuleFor(x => x.RatingTo)
               .LessThan(20);

        }
    }
}
