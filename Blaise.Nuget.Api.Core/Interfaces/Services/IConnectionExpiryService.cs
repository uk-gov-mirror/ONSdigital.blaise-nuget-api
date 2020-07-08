namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IConnectionExpiryService
    {
        void ResetConnectionExpiryPeriod();
        bool ConnectionHasExpired();
    }
}