using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using IssueTracker.Adapters;
using IssueTracker.Attributes;
using IssueTracker.Services.Models;

namespace IssueTracker.Controllers
{
    public class ListIssuesController : ApiController
    {
        private readonly DocumentDbAdapter<Issue> _docDbAdapter = new DocumentDbAdapter<Issue>();

        [GetRoute("Issues")]
        public async Task<IEnumerable<dynamic>> Get()
        {
            return (await _docDbAdapter.ListItems()).Select(issue => new { Uri = string.Format("Issues/{0}", issue.IssueId)});
        }
    }
}