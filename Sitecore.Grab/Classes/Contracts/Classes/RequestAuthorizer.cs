using System;
using System.Linq;
using System.Net;
using Sitecore.Grab.Classes.Contracts.Interfaces;
using Sitecore.Grab.Classes.Domain;
using Sitecore.StringExtensions;

namespace Sitecore.Grab.Classes.Contracts.Classes
{
  public class RequestAuthorizer : IAuthorizer
  {
    private readonly ICheckRequests _checkRequests;
    private readonly PackegeExportSettings _packageExportSettings;
    private readonly ILog _logger;


    public RequestAuthorizer(ICheckRequests checkRequests, PackegeExportSettings packageExportSettings, ILog logger)
    {
      _checkRequests = checkRequests;
      _packageExportSettings = packageExportSettings;
      _logger = logger;
    }


    public bool IsAllowed()
    {
      var isEnabled = _packageExportSettings.IsEnabled;
      if (!_packageExportSettings.IsEnabled)
      {
        LogAccessDenial("packageExportSettings 'enabled' setting is false");
      }

      var isLocalOrAllowRemote = (_checkRequests.IsLocal) || (_packageExportSettings.AllowRemoteAccess);
      if ((!_checkRequests.IsLocal) && (!_packageExportSettings.AllowRemoteAccess))
      {
        LogAccessDenial("packageInstallation 'allowRemote' setting is false");
      }

      var foundAddress = true;
      if (_packageExportSettings.HasAddressWhitelist)
      {

        var matchedRange = _packageExportSettings.AddressWhitelist.FirstOrDefault(ipr => ipr.Value.Contains(IPAddress.Parse(_checkRequests.UserHostAddress)));
        foundAddress = (!matchedRange.Key.IsNullOrEmpty());
        if (!foundAddress)
        {
          LogAccessDenial(string.Format("packageExportSettings whitelist is denying access to {0}", _checkRequests.UserHostAddress));
        }
        else
        {
          LogAccessApproval(string.Format("matched whitelist {0} with address {1}", matchedRange.Key, _checkRequests.UserHostAddress));
        }
      }
      return isEnabled && isLocalOrAllowRemote && foundAddress;
    }

    private void LogAccessDenial(string diagnostic)
    {
      _logger.Write(string.Format("Sitecore.Grab access denied: {0}", diagnostic));
    }

    private void LogAccessApproval(string diagnostic)
    {
      _logger.Write(string.Format("Sitecore.Grab access approved: {0}", diagnostic));
    }
  }
}
