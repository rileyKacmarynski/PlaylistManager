using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Common.Filters.cs;
using Core.Playlists.CreatePlaylist;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;

namespace API.Common
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilterAttribute));
            })
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreatePlaylistCommandValidator>())
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            return services;
        }
    }
}
