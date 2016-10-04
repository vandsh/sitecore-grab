using Sitecore.Grab.Classes.Bases;
using Sitecore.Grab.Classes.Contracts.Interfaces;

namespace Sitecore.Grab.Classes.Domain
{
    public class AboutModule : GrabBaseModule
    {
        public AboutModule(IAuthorizer authoriser)
            : base(authoriser, "/services/about")
        {
            Get["/"] = parameters => View["about.html"];
        }
    }
}