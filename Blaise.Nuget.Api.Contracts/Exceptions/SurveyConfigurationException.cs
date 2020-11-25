using System;

namespace Blaise.Nuget.Api.Contracts.Exceptions
{
    public class SurveyConfigurationException : Exception
    {
        public SurveyConfigurationException(string message) : base(message)
        {
        }
    }
}
