using System;
using System.Globalization;
using Nancy;
using Sitecore.Grab.Classes.Contracts.Interfaces;

namespace Sitecore.Grab.Classes.Bases
{
    public abstract class GrabBaseModule : NancyModule
    {
        private readonly IAuthorizer _authorizer;

        const string StartTime = "start_time";

        protected GrabBaseModule(IAuthorizer authorizer, string modulePath)
            : base(modulePath)
        {
            _authorizer = authorizer;

            Before += AuthorizeRequest;

            Before += ctx =>
            {
                ctx.Items.Add(StartTime, DateTime.UtcNow);
                return null;
            };

            After += AddProcessingTimeToResponse;
        }

        private Response AuthorizeRequest(NancyContext ctx)
        {
            return !_authorizer.IsAllowed() ? new Response { StatusCode = HttpStatusCode.Unauthorized } : null;
        }
        private static void AddProcessingTimeToResponse(NancyContext ctx)
        {
            if (!ctx.Items.ContainsKey(StartTime)) return;

            var processTime = (DateTime.UtcNow - (DateTime)ctx.Items[StartTime]).TotalMilliseconds;

            ctx.Response.WithHeader("x-processing-time", processTime.ToString(CultureInfo.InvariantCulture));
        }
    }
}
