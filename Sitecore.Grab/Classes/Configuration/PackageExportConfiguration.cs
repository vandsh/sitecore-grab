using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Nancy;
using Nancy.Extensions;
using NetTools;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.SecurityModel;
namespace Sitecore.Grab.Classes.Configuration
{
  class PackageExportConfiguration
  {
    private static PackageExportConfiguration _configuration;

    public static PackageExportConfiguration GetConfiguration()
    {
      return _configuration ?? new PackageExportConfiguration();
    }
    public PackageExportConfiguration()
    {
      using (new SecurityDisabler())
      {
        Database authorityDatabase = Sitecore.Configuration.Factory.GetDatabase("master");
        var grabSettings = authorityDatabase.GetItem("/sitecore/system/Settings/Grab Settings");
        CheckboxField enabledField = grabSettings.Fields["Enabled"];
        this._enabled = enabledField.Checked;
        CheckboxField allowRemoteField = grabSettings.Fields["Allow Remote"];
        this._allowRemote = allowRemoteField.Checked;
        var whitelistNameValueCollection = ((NameValueListField)grabSettings.Fields["Whitelist"]).NameValues;
        this._whitelist = whitelistNameValueCollection.AllKeys.ToDictionary(k => k, k => IPAddressRange.Parse(whitelistNameValueCollection[k]));
      }    }

    private bool _enabled = false;    public bool Enabled
    {
      get { return GetConfiguration()._enabled; }
    }
    private bool _allowRemote = false;    public bool AllowRemoteAccess
    {
      get { return GetConfiguration()._allowRemote; }
    }
    private Dictionary<string, IPAddressRange> _whitelist = new Dictionary<string, IPAddressRange>();    public Dictionary<string, IPAddressRange> Whitelist
    {
      get { return GetConfiguration()._whitelist; }
    }
  }
}
