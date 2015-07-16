using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights;

namespace IssueTracker
{
    public class AppInsightsExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            new TelemetryClient().TrackException(context.Exception);
        }
    }
}