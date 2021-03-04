using System;

namespace Blaise.Nuget.Api.Contracts.Exceptions
{
    public class MethodFailedException : Exception
    {
        public MethodFailedException(string message) : base(message)
        {
        }
    }
}
