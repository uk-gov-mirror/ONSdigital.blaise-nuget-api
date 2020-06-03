using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseUserApi
    {
        void AddUser(string password, string role, IList<string> serverParkNames);

        void ChangePassword(string password);
    }
}