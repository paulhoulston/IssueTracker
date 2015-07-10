using System;
using System.Threading.Tasks;

namespace IssueTracker.Services
{
    public class IssueCreationService
    {
        private readonly ICreateIssues _issueCreator;

        public interface ICreateIssues
        {
            Task CreateIssue(Issue issue);
        }

        public class Issue
        {
            public string CreatedBy { get; set; }
            public DateTime CreatedTime { get; set; }
            public int IssueId { get; set; }
        }

        public IssueCreationService(ICreateIssues issueCreator)
        {
            _issueCreator = issueCreator;
        }

        public async Task CreateIssue(string createdBy)
        {
            await _issueCreator.CreateIssue(new Issue
            {
                CreatedBy = createdBy,
                CreatedTime = DateTime.UtcNow
            });
        }
    }
}