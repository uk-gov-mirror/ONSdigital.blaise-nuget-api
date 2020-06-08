using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseUserApi
    {
        IFluentBlaiseUserApi WithUserName(string userName);

        IFluentBlaiseUserApi WithPassword(string password);

        IFluentBlaiseUserApi WithRole(string role);

        IFluentBlaiseUserApi WithServerParks(IList<string> serverParkNames);

        void Add();

        void Update();

        void Remove();

        bool Exists { get; }
    }
}