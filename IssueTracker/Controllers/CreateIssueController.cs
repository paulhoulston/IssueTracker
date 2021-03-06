using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IssueTracker.Adapters;
using IssueTracker.Attributes;
using IssueTracker.Services;

namespace IssueTracker.Controllers
{
    public class CreateIssueController : ApiController
    {
        private readonly IssueCreationService _service = new IssueCreationService(new IssueRepository(), new IssueIdRepository());

        public class Issue
        {
            public string CreatedBy { get; set; }
        }

        [PostRoute("Issues")]
        public async Task<HttpResponseMessage> Post(Issue issue)
        {
            HttpResponseMessage response = null;
            await _service.CreateIssue(
                issue.CreatedBy,
                item => response = Request.CreateResponse(HttpStatusCode.Created, new {Uri = string.Format("/Issues/{0}", item.IssueId)}));
            return response;
        }
    }
}