using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IDataModelService
    {
        IDatamodel GetDataModel(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IDatamodel GetDataModel(string databaseFile);

        SurveyType GetSurveyType(ConnectionModel connectionModel, string instrumentName, string serverParkName);
    }
}