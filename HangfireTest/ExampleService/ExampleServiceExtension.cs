using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleServiceLibrary
{
    public static class ExampleServiceExtension
    {
        public static void AddExampleService(
            this IServiceCollection services,
            bool testBoolean,
            string defaultMessage)
        {
            services.AddScoped<ExampleService>(serviceProvider =>
            {
                return new ExampleService(testBoolean, defaultMessage);
            });
        }
    }
}
