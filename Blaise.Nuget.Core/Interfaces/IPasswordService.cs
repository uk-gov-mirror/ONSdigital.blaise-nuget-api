
using System.Security;

namespace Blaise.Nuget.Core.Interfaces
{
    public interface IPasswordService
    {
        SecureString CreateSecurePassword(string password);
    }
}
