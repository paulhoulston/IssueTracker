using System;
using System.Threading.Tasks;
using IssueTracker.Adapters;

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

        [DocumentCollectionId("Issues")]
        public class Issue : IDocumentItem
        {
            public string Id { get; private set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedTime { get; set; }
            public int IssueId { get; set; }
        }

        [DocumentCollectionId("IssueIds")]
        public class IssueId : IDocumentItem
        {
            public string Id { get; set; }
            public int CurrentIssueId { get; set; }
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