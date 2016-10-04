namespace Sitecore.Grab.Classes.Contracts.Interfaces
{
    public interface IConfigurationProvider<out T>
    {
        T Settings { get; }
    }
}
