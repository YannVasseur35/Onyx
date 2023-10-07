using Microsoft.Extensions.DependencyInjection;
using Onyx.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Application
{
    public static class ServicesBuilder
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPlayerJourneyAppServices, PlayerJourneyAppServices>();

            return services;
        }
    }
}