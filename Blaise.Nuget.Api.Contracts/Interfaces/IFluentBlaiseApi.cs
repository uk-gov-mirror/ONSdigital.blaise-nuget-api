﻿using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseApi
    {
        IFluentBlaiseApi Server(string serverName);

        IFluentBlaiseApi Instrument(string instrumentName);

        IFluentBlaiseApi ServerPark(string serverParkName);

        IFluentBlaiseCaseApi Case(string primaryKeyValue);

        IFluentBlaiseCaseApi Case(IDataRecord caseDataRecord);

        IFluentBlaiseCaseApi Case();

        IFluentBlaiseUserApi User(string userName);

        IFluentBlaiseSurveyApi Survey();

        bool Exists();
    }
}
