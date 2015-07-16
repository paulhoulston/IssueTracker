using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IssueTracker.Adapters;
using IssueTracker.Attributes;
using IssueTracker.Services;
using log4net;

namespace IssueTracker.Controllers
{
    public class CreateIssueController : ApiController
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CreateIssueController));

        private readonly IssueCreationService _service = new IssueCreationService(new IssueRepository(), new IssueIdRepository());

        public class Issue
        {
            public string CreatedBy { get; set; }
        }

        [PostRoute("Issues")]
        public async Task<HttpResponseMessage> Post(Issue issue)
        {
            HttpResponseMessage response = null;
            try
            {
                await _service.CreateIssue(
                    issue.CreatedBy,
                    item => response = Request.CreateResponse(HttpStatusCode.Created, new {Uri = string.Format("/Issues/{0}", item.IssueId)}));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }
    }
}