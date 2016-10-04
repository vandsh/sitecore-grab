using Sitecore.Grab.Classes.Contracts.Interfaces;

namespace Sitecore.Grab.Classes.Contracts.Classes
{
    public class Logger : ILog
    {
        public void Write(string message)
        {
            Sitecore.Diagnostics.Log.Warn(message, this);
        }
    }
}
