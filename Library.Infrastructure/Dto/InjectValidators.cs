using FluentValidation;
using Library.Infrastructure.Dto.LibraryDto;
using Library.Infrastructure.Dto.UserDto;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dto
{
    public static class InjectValidators
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<UserLogInDto>, UserLogInDtoValidator>();
            services.AddScoped<IValidator<UserUpdateDto>, UserUpdateDtoValidator>();
            services.AddScoped<IValidator<AuthorDto>, AuthorDtoValidator>();
            services.AddScoped<IValidator<BookDto>, BookDtoValidator>();
            services.AddScoped<IValidator<FilterBookDto>, FilterBookDtoValidator>();
            services.AddScoped<IValidator<UpdateBookDto>, UpdateBookDtoValidator>();

            return services;
        }
    }
}
