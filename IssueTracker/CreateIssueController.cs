using System.Net;
using System.Net.Http;
using System.Web.Http;
using IssueTracker.Adapters;
using IssueTracker.Services;

namespace IssueTracker
{
    public class CreateIssueController : ApiController
    {
        public class Issue
        {
            public string CreatedBy { get; set; }
        }

        [HttpPost, Route("Issues")]
        public HttpResponseMessage Post(Issue issue)
        {
            try
            {
                IssueCreationService.ICreateIssues repository = new IssueRepository();
                new IssueCreationService(repository).CreateIssue(issue.CreatedBy);

                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}