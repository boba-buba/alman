using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alman;

public static class AlmanConfig
{
    static IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

    public static string GetConfigString(string key)
    {
        string? configValue = configuration.GetSection(key).Value;
        if (configValue is null)
        {
            return string.Empty;
        }
        return configValue;
    }

}
