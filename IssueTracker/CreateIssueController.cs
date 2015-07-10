using System;
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
        private readonly IssueCreationService.IGetEventIds _idGetter = new IssueIdRepository();

        public class Issue
        {
            public string CreatedBy { get; set; }
        }

        [HttpPost, Route("Issues")]
        public async Task<HttpResponseMessage> Post(Issue issue)
        {
            var response = new HttpResponseMessage();
            try
            {
                await new IssueCreationService(_issueCreator, _idGetter).CreateIssue(issue.CreatedBy);
                response.StatusCode = HttpStatusCode.Created;
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
#if DEBUG
                response.Content = new StringContent(ex.Message);
#endif
            }

            return response;
        }
    }
}