using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IssueTracker.Adapters;
using IssueTracker.Attributes;
using IssueTracker.Services.Models;

namespace IssueTracker.Controllers
{
    public class GetIssueController : ApiController
    {
        private readonly DocumentDbAdapter<Issue> _docDbAdapter = new DocumentDbAdapter<Issue>();

        [GetRoute("Issues/{issueId:int}")]
        public async Task<HttpResponseMessage> Get(int issueId)
        {
            HttpResponseMessage response = null;
            await _docDbAdapter.GetItem(issue => issue.IssueId == issueId,
                async () => response = Request.CreateResponse(HttpStatusCode.NotFound),
                async issue => response = Request.CreateResponse(HttpStatusCode.OK, issue));
            return response;
        }
    }
}