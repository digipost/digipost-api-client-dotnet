using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Digipost.Api.Client
{
    public class Logging
    {
        private static Action<TraceEventType, Guid?, string, string> _logAction;

        [Obsolete("Initialize is called within the DigipostClient and calls to this method can be removed. NB. This will be removed in future version.")]
        public static void Initialize(ClientConfig konfigurasjon)
        {
            _logAction = konfigurasjon.Logger;
        }

        public static void Log(TraceEventType severity, string message, [CallerMemberName] string callerMember = null)
        {
            Log(severity, null, message, callerMember);
        }

        public static void Log(TraceEventType severity, Guid? conversationId, string message,
            [CallerMemberName] string callerMember = null)
        {
            if (_logAction == null)
            {
                _logAction = ConsoleLogger();
            }

            if (callerMember == null)
            {
                callerMember = new StackFrame(1).GetMethod().Name;
            }

            _logAction(severity, conversationId, callerMember, message);
        }

        public static Action<TraceEventType, Guid?, string, string> TraceLogger()
        {
            var traceSource = new TraceSource("Digipost.Api.Klient");
            return
                (severity, koversasjonsId, caller, message) =>
                {
                    traceSource.TraceEvent(severity, 1, "[{0}, {1}] {2}", koversasjonsId.GetValueOrDefault(), caller,
                        message);
                };
        }

        public static Action<TraceEventType, Guid?, string, string> ConsoleLogger()
        {
            return (severity, koversasjonsId, caller, message) => { Console.WriteLine("[{0}] {1}", caller, message); };
        }
    }
}