using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Domein.Domein.EntityModelBuilders
{
    public static class InjectValidator
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
          
            return services;
        }
    }
}
