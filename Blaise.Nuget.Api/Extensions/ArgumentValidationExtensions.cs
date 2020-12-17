using System;

namespace Blaise.Nuget.Api.Extensions
{
    internal static class ArgumentValidationExtensions
    {
        public static void ThrowExceptionIfNullOrEmpty(this string parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (string.IsNullOrWhiteSpace(parameter))
            {

                throw new ArgumentException($"A value for the argument '{parameterName}' must be supplied");
            }
        }

        public static void ThrowExceptionIfNull<T>(this T parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException($"The argument '{parameterName}' must be supplied");
            }
        }
    }
}
