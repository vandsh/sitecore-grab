using System.Web;
using Sitecore.Grab.Classes.Contracts.Interfaces;

namespace Sitecore.Grab.Classes.Contracts.Classes
{
    public class HttpRequestChecker : ICheckRequests
    {
        public bool IsLocal
        {
            get { return HttpContext.Current.Request.IsLocal; }
        }

        public string UserHostAddress
        {
            get { return HttpContext.Current.Request.UserHostAddress; }
        }
    }
}
