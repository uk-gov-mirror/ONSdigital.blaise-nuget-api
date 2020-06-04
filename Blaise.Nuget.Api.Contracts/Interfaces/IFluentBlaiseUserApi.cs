using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseUserApi
    {


        void Add(string password, string role, IList<string> serverParkNames);

        void Update(string role, IList<string> serverParkNames);

        void ChangePassword(string password);

        void Remove();

        bool Exists();
    }
}