namespace Blaise.Nuget.Api
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Unity.Interception.InterceptionBehaviors;
    using Unity.Interception.PolicyInjection.Pipeline;

    public class LoggingInterceptionBehavior : IInterceptionBehavior
    {
        public bool WillExecute => true;

        public IEnumerable<Type> GetRequiredInterfaces() => Type.EmptyTypes;

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            if (!EventLog.SourceExists("NUGET_LOG"))
            {
                EventLog.CreateEventSource("NUGET_LOG", "Application");
            }

            var args = string.Join(", ", input.Arguments.Cast<object>().Select(a => a?.ToString() ?? "<null>"));
            EventLog.WriteEntry("NUGET_LOG", $"[LOG] Calling {input.MethodBase.Name} with args: {args}");

            // Call the actual method
            var result = getNext()(input, getNext);

            if (result.Exception == null && input.MethodBase is System.Reflection.MethodInfo methodInfo &&
                methodInfo.ReturnType != typeof(void))
            {
                EventLog.WriteEntry("NUGET_LOG", $"[LOG] {input.MethodBase.Name} returned {result.ReturnValue}");
            }
            else if (result.Exception != null)
            {
                EventLog.WriteEntry("NUGET_LOG", $"[LOG] {input.MethodBase.Name} threw exception: {result.Exception.Message}");
            }

            return result;
        }
    }
}
