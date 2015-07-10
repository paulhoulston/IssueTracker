using System.Threading.Tasks;
using IssueTracker.Services;
using IssueTracker.Services.Models;

namespace IssueTracker.Adapters
{
    internal class IssueRepository : IssueCreationService.ICreateIssues
    {
        private readonly DocumentDbAdapter<Issue> _docDbAdapter = new DocumentDbAdapter<Issue>();

        public async Task CreateIssue(Issue issue)
        {
            await _docDbAdapter.AddItem(issue);
        }
    }
}