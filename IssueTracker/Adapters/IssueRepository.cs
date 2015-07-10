using System.Threading.Tasks;
using IssueTracker.Services;

namespace IssueTracker.Adapters
{
    internal class IssueRepository : IssueCreationService.ICreateIssues
    {
        private readonly DocumentDbAdapter<IssueCreationService.Issue> _docDbAdapter = new DocumentDbAdapter<IssueCreationService.Issue>();

        public async Task CreateIssue(IssueCreationService.Issue issue)
        {
            await _docDbAdapter.AddItem(issue);
        }
    }
}