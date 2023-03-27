using FluentValidation;
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
           
            return services;
        }
    }
}
