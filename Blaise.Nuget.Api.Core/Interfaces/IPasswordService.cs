
using System.Security;

namespace Blaise.Nuget.Api.Core.Interfaces
{
    public interface IPasswordService
    {
        SecureString CreateSecurePassword(string password);
    }
}
