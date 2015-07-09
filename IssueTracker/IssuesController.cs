using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IssueTracker
{
    public class IssuesController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}