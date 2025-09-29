namespace Blaise.Nuget.Api
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Security;
    using Unity.Interception.InterceptionBehaviors;
    using Unity.Interception.PolicyInjection.Pipeline;

    public class LoggingInterceptionBehavior : IInterceptionBehavior
    {
        public bool WillExecute => true;

        public IEnumerable<Type> GetRequiredInterfaces() => Type.EmptyTypes;

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            const string source = "NUGET_LOG";

            TryLog(() =>
            {
                var args = string.Join(", ", input.Arguments.Cast<object>().Select(a => a?.ToString() ?? "<null>"));
                EventLog.WriteEntry(source, $"[LOG] Calling {input.MethodBase.Name} with args: {args}");
            });

            var result = getNext()(input, getNext);

            TryLog(() =>
            {
                if (result.Exception == null && input.MethodBase is System.Reflection.MethodInfo methodInfo &&
                    methodInfo.ReturnType != typeof(void))
                {
                    EventLog.WriteEntry(source, $"[LOG] {input.MethodBase.Name} returned {result.ReturnValue}");
                }
                else if (result.Exception != null)
                {
                    EventLog.WriteEntry(source, $"[LOG] {input.MethodBase.Name} threw exception: {result.Exception.Message}");
                }
            });

            return result;
        }

        private void TryLog(Action logAction)
        {
            try
            {
                logAction();
            }
            catch
            {
                Console.WriteLine("EventLog write failed.");
            }
        }
    }
}
