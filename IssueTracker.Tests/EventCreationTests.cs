using NUnit.Framework;

namespace IssueTracker.Tests
{
    internal class Given_I_want_to_create_an_issue
    {
        private class When_I_create_an_issue : IssueCreationService.ICreateIssues
        {
            private bool _issueCreated;

            [Test]
            public void Then_the_issue_is_created()
            {
                new IssueCreationService(this).CreateIssue();
                Assert.IsTrue(_issueCreated);
            }

            public void CreateIssue()
            {
                _issueCreated = true;
            }
        }
    }

    internal class IssueCreationService
    {
        private readonly ICreateIssues _issueCreator;

        public interface ICreateIssues
        {
            void CreateIssue();
        }

        public IssueCreationService(ICreateIssues issueCreator)
        {
            _issueCreator = issueCreator;
        }

        public void CreateIssue()
        {
            _issueCreator.CreateIssue();
        }
    }
}