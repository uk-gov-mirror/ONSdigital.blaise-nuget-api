using Blaise.Nuget.Core.Interfaces;
using System.Security;

namespace Blaise.Nuget.Core.Services
{
    public class PasswordService : IPasswordService
    {
        public SecureString CreateSecurePassword(string password)
        {
            char[] passwordChars = password.ToCharArray();
            SecureString securePassword = new SecureString();
            foreach (char c in passwordChars)

            {
                securePassword.AppendChar(c);
            }
            return securePassword;
        }
    }
}
