using System;
using System.Threading.Tasks;

namespace IssueTracker.Services
{
    public class IssueCreationService
    {
        private readonly ICreateIssues _issueCreator;
        private readonly IGetEventIds _eventIdGetter;

        public interface ICreateIssues
        {
            Task CreateIssue(Issue issue);
        }

        public interface IGetEventIds
        {
            Task<int> GetNextId();
        }

        public class Issue
        {
            public string CreatedBy { get; set; }
            public DateTime CreatedTime { get; set; }
            public int IssueId { get; set; }
        }

        public IssueCreationService(ICreateIssues issueCreator, IGetEventIds eventIdGetter)
        {
            _issueCreator = issueCreator;
            _eventIdGetter = eventIdGetter;
        }

        public async Task CreateIssue(string createdBy)
        {
            await _issueCreator.CreateIssue(new Issue
            {
                IssueId = await _eventIdGetter.GetNextId(),
                CreatedBy = createdBy,
                CreatedTime = DateTime.UtcNow
            });
        }
    }
}