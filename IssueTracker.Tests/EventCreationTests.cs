using NUnit.Framework;

namespace IssueTracker.Tests
{
    class Given_I_want_to_create_an_issue
    {
        class When_I_create_an_issue
        {
            [Test]
            public void Then_the_issue_is_created()
            {
                Assert.IsTrue(new IssueCreationService().CreateIssue());
            }
        }
    }

    class IssueCreationService
    {
        public bool CreateIssue()
        {
            return true;
        }
    }
}
