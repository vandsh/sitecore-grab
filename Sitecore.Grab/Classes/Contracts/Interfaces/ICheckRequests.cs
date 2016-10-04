namespace Sitecore.Grab.Classes.Contracts.Interfaces
{
    public interface ICheckRequests
    {
        bool IsLocal { get; }

        string UserHostAddress { get; }
    }
}
