using Sitecore.Grab.Classes.Configuration;
using Sitecore.Grab.Classes.Contracts.Interfaces;
using Sitecore.Grab.Classes.Domain;

namespace Sitecore.Grab.Classes.Contracts.Classes
{
    public class PackageExportConfigurationProvider : IConfigurationProvider<PackegeExportSettings>
    {
        public PackageExportConfigurationProvider()
        {
            var config = PackageExportConfiguration.GetConfiguration();

            Settings = new PackegeExportSettings()
            {
                IsEnabled = config.Enabled,
                AllowRemoteAccess = config.AllowRemoteAccess,
                AllowPackageStreaming = config.AllowPackageStreaming,
                RecordInstallationHistory = config.RecordInstallationHistory,
                MuteAuthorisationFailureLogging = config.MuteAuthorisationFailureLogging
            };

            if (config.Whitelist.Count > 0)
            {
                foreach (var item in config.Whitelist)
                {
                    Settings.AddressWhitelist.Add(item.IP);
                }
            }
        }

        public PackegeExportSettings Settings { get; private set; }
    }
}
