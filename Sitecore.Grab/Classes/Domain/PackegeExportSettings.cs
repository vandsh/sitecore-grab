using System.Collections.Generic;
using NetTools;

namespace Sitecore.Grab.Classes.Domain
{
    public class PackegeExportSettings
    {
        public PackegeExportSettings()
        {
            IsEnabled = false;
            AllowRemoteAccess = false;
            AddressWhitelist = new Dictionary<string, IPAddressRange>();
        }

        public bool IsEnabled { get; set; }
        public bool AllowRemoteAccess { get; set; }
        public Dictionary<string, IPAddressRange> AddressWhitelist { get; set; }
        public bool HasAddressWhitelist { get { return AddressWhitelist.Count > 0; } }
    }
}
