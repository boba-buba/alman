using Microsoft.Extensions.Configuration;

namespace Alman;

/// <summary>
///  Application configuration class. Provides API for getting configuration strings from appsettings.json.
/// </summary>
public static class AlmanConfig
{
    /// <summary>
    /// Root of the configuration tree.
    /// </summary>
    static IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

    /// <summary>
    /// Get value of the configuration parameter from configuration.
    /// </summary>
    /// <param name="key"> key for the configuration parameter. </param>
    /// <returns> Value of the parameter or empty string if not found. </returns>
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
