using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseUserApi
    {
        void Add(string password, string role, IEnumerable<string> serverParkNames);

        void ChangePassword(string password);

        bool UserExists();

        void RemoveUser();
    }
}