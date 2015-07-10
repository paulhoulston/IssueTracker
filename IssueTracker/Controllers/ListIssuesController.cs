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
            return new dynamic[] { new { Item = "item 1" }, new { Item = "item 2" }, new { Item = "item 3" } };
        }
    }
}