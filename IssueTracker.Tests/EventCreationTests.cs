using NUnit.Framework;

namespace IssueTracker.Tests
{
    internal class Given_I_want_to_create_an_issue
    {
        private class When_I_create_an_issue : IssueCreationService.ICreateIssues
        {
            private bool _issueCreated;
            private string _issueCreator;
            
            private const string CreatedBy = "Paul Houslton";

            public When_I_create_an_issue()
            {
                new IssueCreationService(this).CreateIssue(CreatedBy);
            }

            [Test]
            public void Then_the_issue_is_created()
            {
                Assert.IsTrue(_issueCreated);
            }

            [Test]
            public void And_the_creator_is_assigned_to_the_event()
            {
                Assert.AreEqual(CreatedBy, _issueCreator);
            }

            public void CreateIssue(string createdBy)
            {
                _issueCreator = createdBy;
                _issueCreated = true;
            }
        }
    }

    internal class IssueCreationService
    {
        private readonly ICreateIssues _issueCreator;

        public interface ICreateIssues
        {
            void CreateIssue(string createdBy);
        }

        public IssueCreationService(ICreateIssues issueCreator)
        {
            _issueCreator = issueCreator;
        }

        public void CreateIssue(string createdBy)
        {
            _issueCreator.CreateIssue(createdBy);
        }
    }
}