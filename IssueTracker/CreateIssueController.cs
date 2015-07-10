using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IssueTracker.Adapters;
using IssueTracker.Services;

namespace IssueTracker
{
    public class CreateIssueController : ApiController
    {
        private readonly IssueCreationService.ICreateIssues _issueCreator = new IssueRepository();

        public class Issue
        {
            public string CreatedBy { get; set; }
        }

        [HttpPost, Route("Issues")]
        public async Task<HttpResponseMessage> Post(Issue issue)
        {
            try
            {
                await new IssueCreationService(_issueCreator).CreateIssue(issue.CreatedBy);

                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}