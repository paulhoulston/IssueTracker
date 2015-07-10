using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using IssueTracker.Adapters;
using IssueTracker.Services.Models;

namespace IssueTracker
{
    public class GetIssueController : ApiController
    {
        private readonly DocumentDbAdapter<Issue> _docDbAdapter = new DocumentDbAdapter<Issue>();

        [HttpGet, Route("Issues/{issueId:int}")]
        public async Task<HttpResponseMessage> Get(int issueId)
        {
            var response = new HttpResponseMessage();
            await _docDbAdapter.GetItem(issue => issue.IssueId == issueId,
                async () => response.StatusCode = HttpStatusCode.NotFound,
                async issue =>
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Content = new ObjectContent(issue.GetType(), issue, new JsonMediaTypeFormatter());
                });

            return response;
        }
    }
}