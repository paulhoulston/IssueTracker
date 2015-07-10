using System;
using System.Threading.Tasks;
using IssueTracker.Services.Models;

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

        public IssueCreationService(ICreateIssues issueCreator, IGetEventIds eventIdGetter)
        {
            _issueCreator = issueCreator;
            _eventIdGetter = eventIdGetter;
        }

        public async Task CreateIssue(string createdBy, Action<Issue> onIssueCreated)
        {
            var issue = new Issue
            {
                IssueId = await _eventIdGetter.GetNextId(),
                CreatedBy = createdBy,
                CreatedTime = DateTime.UtcNow
            };
            await _issueCreator.CreateIssue(issue);

            onIssueCreated(issue);
        }
    }
}