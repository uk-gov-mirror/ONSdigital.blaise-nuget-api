using Blaise.Nuget.Api.Core.Interfaces.Services;
using System.Security;

namespace Blaise.Nuget.Api.Core.Services
{ 
    public class PasswordService : IPasswordService
    {
        public SecureString CreateSecurePassword(string password)
        {
            var passwordChars = password.ToCharArray();
            var securePassword = new SecureString();
            foreach (var c in passwordChars)

            {
                securePassword.AppendChar(c);
            }
            return securePassword;
        }
    }
}
