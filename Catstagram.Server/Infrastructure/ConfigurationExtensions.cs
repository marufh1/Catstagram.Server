using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catstagram.Server.Infrastructure
{
    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnecionString(this IConfiguration configuration)
            => configuration.GetConnectionString("DefaultConnection");
        
       
    }
}
