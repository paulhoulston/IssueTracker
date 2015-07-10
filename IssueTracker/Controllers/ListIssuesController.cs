using System.Threading.Tasks;
using System.Web.Http;
using IssueTracker.Attributes;

namespace IssueTracker.Controllers
{
    public class ListIssuesController : ApiController
    {
        [GetRoute("Issues")]
        public async Task<dynamic> Get()
        {
            return new dynamic[] { new { item = "item 1" }, new { item = "item 2" }, new { item = "item 3" } };
        }
    }
}