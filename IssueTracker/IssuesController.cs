using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IssueTracker.Services;

namespace IssueTracker
{
    public class IssuesController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }

    public class CreateIssueController : ApiController
    {
        public class Issue
        {
            public string CreatedBy { get; set; }    
        }

        [HttpPost, Route("Issues")]
        public async Task<HttpResponseMessage> Post(Issue issue)
        {
            try
            {
                IssueCreationService.ICreateIssues repository = null;
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